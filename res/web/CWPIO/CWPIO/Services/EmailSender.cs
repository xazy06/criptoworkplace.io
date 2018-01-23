using CWPIO.Models;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CWPIO.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        MailjetClient _client;
        public EmailSender(IOptions<MailSettings> mailSettings)
        {
            _client = new MailjetClient(mailSettings.Value.ApiKey, mailSettings.Value.ApiSecret);
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string message, string html = null)
        {

            MailjetRequest request = new MailjetRequest() { Resource = Send.Resource }
                .Property(Send.FromEmail, "robot@cryptoworkplace.io")
                .Property(Send.FromName, "CryptoWorkPlace Robot")
                .Property(Send.Subject, subject)
                .Property(Send.TextPart, message);
            if (!string.IsNullOrEmpty(html))
                request.Property(Send.HtmlPart, html);

            request.Property(Send.Recipients, new JArray {
                    new JObject { {"Email", email } }
                    });


            MailjetResponse response = await _client.GetAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
