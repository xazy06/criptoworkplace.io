using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using CWPIO.Data;
using CWPIO.Models;
using CWPIO.Models.ManageViewModels;
using CWPIO.Services;
using DnsClient;
using DnsClient.Protocol;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Slack.Webhooks;


namespace CWPIO.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private IStringLocalizer<HomeController> _localizer;
        private IEmailSender _emailSender;
        private ISlackClient _slack;

        public HomeController(ApplicationDbContext context,
            IStringLocalizer<HomeController> localizer,
            IEmailSender emailSender,
            ISlackClient slack)
        {
            _context = context;
            _localizer = localizer;
            _emailSender = emailSender;
            _slack = slack;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(SubscribeViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
                return Json(new { result = false, Error = _localizer["Subscribe_NoName"] });
            if (string.IsNullOrEmpty(model.Email) || !(await IsValidAsync(model.Email)))
                return Json(new { result = false, Error = _localizer["Subscribe_NoEmail"] });

            var postToSlack = false;
            var dbSet = _context.Set<Subscriber>();
            var entry = await dbSet.FirstOrDefaultAsync(s => s.Email == model.Email);
            if (entry == null)
            {
                entry = (await dbSet.AddAsync(new Subscriber
                {
                    Name = model.Name,
                    Email = model.Email,
                    EmailSend = true,
                    DateCreated = DateTime.Now,
                    Culture = CultureInfo.CurrentUICulture.ToString()
                })).Entity;
                postToSlack = true;

            }
            else
            {
                if (entry.Name != model.Name)
                    entry.Name = model.Name;
                if (entry.Unsubscribe)
                {
                    entry.Unsubscribe = false;
                    postToSlack = true;
                }
            }

            await _context.SaveChangesAsync();

            if (postToSlack)
            {
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
            }
            var sendResult = await _emailSender.SendEmailSubscription(model.Email, model.Name);

            return Json(new { result = sendResult, Error = "" });
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
