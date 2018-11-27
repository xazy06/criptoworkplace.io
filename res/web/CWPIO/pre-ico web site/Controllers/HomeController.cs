using DnsClient;
using DnsClient.Protocol;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nethereum.Util;
using pre_ico_web_site.Data;
using pre_ico_web_site.Eth;
using pre_ico_web_site.Models;
using pre_ico_web_site.Services;
using Slack.Webhooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace pre_ico_web_site.Controllers
{
    public class HomeController : Controller
    {
        private readonly TokenSaleContract _contract;
        private ApplicationDbContext _context;
        private IEmailSender _emailSender;
        private ISlackClient _slack;
        public HomeController(TokenSaleContract contract,
            ApplicationDbContext context,
            IEmailSender emailSender,
            ISlackClient slack)
        {
            _contract = contract;
            _context = context;
            _emailSender = emailSender;
            _slack = slack;
        }

        public async Task<IActionResult> Index()
        {
            int sold, cap;
            try
            {
                sold = (int)UnitConversion.Convert.FromWei(await _contract.TokenSoldAsync());
                sold = sold > 0 ? sold : 8031923;
                cap = (int)UnitConversion.Convert.FromWei(await _contract.GetCapAsync());
                cap = cap > 0 ? cap : 15000000;
            }
            catch
            {
                sold = 8031923;
                cap = 15000000;
            }
            var model = new BarModel
            {
                Sold = sold,
                Cap = cap
            };
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(SubscribeViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || !(await IsValidAsync(model.Email)))
            {
                return Json(new { result = false, Error = "Email field is empty" });
            }

            var dbSet = _context.Set<Subscriber>();
            var entry = await dbSet.FirstOrDefaultAsync(s => s.Email == model.Email);
            if (entry == null)
            {
                entry = (await dbSet.AddAsync(new Subscriber
                {
                    Name = model.Email,
                    Email = model.Email,
                    EmailSend = true,
                    DateCreated = DateTime.Now,
                    Culture = CultureInfo.CurrentUICulture.ToString()
                })).Entity;
            }
            else
            {
                if (entry.Unsubscribe)
                {
                    entry.Unsubscribe = false;
                }
                else
                {
                    return Json(new { result = false, Error = "User is already subscribed to the newsletter" });
                }
            }

            await _context.SaveChangesAsync();

            _slack.Post(new SlackMessage
            {
                Attachments = new List<SlackAttachment> {
                    new SlackAttachment
                    {
                        Color = "#120a8f",
                        Title = $"Подписка пользователя {entry.Name}",
                        TitleLink = $"mailto:{entry.Email}",
                        Fields = new List<SlackField>
                        {
                            new SlackField{
                                Title = $"Подписка: {(entry.Unsubscribe ? "Нет":"Да")}",
                                Value = $"Email: {entry.Email}",
                                Short = false
                            }
                        },
                        Pretext = $"Дата регистрации: {entry.DateCreated.ToString("dd:MM:yyyy HH:mm")}"
                    }
                }
            });
            //var sendResult = await _emailSender.SendEmailSubscription(model.Email, model.Email);

            return Json(new { result = true/*sendResult*/, Error = "" });
        }
        public async Task<IActionResult> Unsubscribe(string email)
        {
            var dbSet = _context.Set<Subscriber>();

            var entry = await dbSet.FirstOrDefaultAsync(s => s.Email == email);
            if (entry == null)
            {
                return View(new UnsubscribeViewModel { Result = false });
            }

            entry.Unsubscribe = true;
            await _context.SaveChangesAsync();

            _slack.Post(new SlackMessage
            {
                Attachments = new List<SlackAttachment> {
                    new SlackAttachment
                    {
                        Color = "#ff6347",
                        Title = $"Отписка пользователя {entry.Name}",
                        TitleLink = $"mailto:{entry.Email}",
                        Fields = new List<SlackField>
                        {
                            new SlackField{
                                Title = $"Подписка: {(entry.Unsubscribe ? "Нет":"Да")}",
                                Value = $"Email: {entry.Email}",
                                Short = false
                            }
                        },
                        Pretext = $"Дата регистрации: {entry.DateCreated.ToString("dd:MM:yyyy HH:mm")}"
                    }
                }
            });

            return View(new UnsubscribeViewModel { Result = true, Email = email });
        }


        private Task<bool> IsValidAsync(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                var host = mailAddress.Host;
                return CheckDnsEntriesAsync(host);
            }
            catch (FormatException)
            {
                return Task.FromResult(false);
            }
        }

        private async Task<bool> CheckDnsEntriesAsync(string domain)
        {
            try
            {
                var lookup = new LookupClient
                {
                    Timeout = TimeSpan.FromSeconds(5)
                };
                var result = await lookup.QueryAsync(domain, QueryType.ANY).ConfigureAwait(false);

                var records = result.Answers.Where(record => record.RecordType == ResourceRecordType.MX);
                return records.Any();
            }
            catch (DnsResponseException)
            {
                return false;
            }
        }
    }
}