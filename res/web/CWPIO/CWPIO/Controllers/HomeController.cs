using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CWPIO.Models;
using CWPIO.Data;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using DnsClient;
using CWPIO.Services;

namespace CWPIO.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private IStringLocalizer<HomeController> _localizer;
        private IEmailSender _emailSender;
        public HomeController(ApplicationDbContext context, IStringLocalizer<HomeController> localizer, IEmailSender emailSender)
        {
            _context = context;
            _localizer = localizer;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
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
            if (string.IsNullOrEmpty(model.Name))
                return Json(new { result = false, Error = _localizer["Subscribe_NoName"] });
            if (string.IsNullOrEmpty(model.Email) || !(await IsValidAsync(model.Email)))
                return Json(new { result = false, Error = _localizer["Subscribe_NoEmail"] });

            var dbSet = _context.Set<Subscriber>();
            if (await dbSet.AnyAsync(s => s.Email == model.Email))
                return Json(new { result = false, Error = _localizer["Subscribe_EmailExist"] });
            await dbSet.AddAsync(new Subscriber { Name = model.Name, Email = model.Email });
            await _emailSender.SendEmailSubscription(model.Email, model.Name);
            return Json(new { result = true, Error = "" });
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

                var records = result.Answers.Where(record => record.RecordType == DnsClient.Protocol.ResourceRecordType.MX);
                return records.Any();
            }
            catch (DnsResponseException)
            {
                return false;
            }
        }
    }
}
