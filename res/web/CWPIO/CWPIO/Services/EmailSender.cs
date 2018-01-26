using CWPIO.Models;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
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
        private IStringLocalizer<EmailSender> _localizer;
        private MailjetClient _client;
        private IOptions<MailSettings> _mailSettings;
        public EmailSender(IOptions<MailSettings> mailSettings, IStringLocalizer<EmailSender> localizer)
        {
            _client = new MailjetClient(mailSettings.Value.ApiKey, mailSettings.Value.ApiSecret);
            _localizer = localizer;
            _mailSettings = mailSettings;
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string message, string html = null)
        {

            MailjetRequest request = new MailjetRequest() { Resource = Send.Resource }
                .Property(Send.FromEmail, "info@cryptoworkplace.io")
                .Property(Send.FromName, "CryptoWorkPlace Info")
                .Property(Send.Subject, subject)
                .Property(Send.TextPart, message);
            if (!string.IsNullOrEmpty(html))
                request.Property(Send.HtmlPart, html);

            request.Property(Send.Recipients, new JArray {
                    new JObject { {"Email", email } }
                    });


            MailjetResponse response = await _client.PostAsync(request);
            return response.IsSuccessStatusCode;
        }

        public Task<bool> SendEmailSubscription(string email, string name)
        {
            var title = _localizer["Subscribe_Title"];
            var mailto_text = _localizer["Subscribe_Mailto_Text"];
            var view_browser = _localizer["Subscribe_View_Browser"];
            var topic = _localizer["Subscribe_Topic"];
            var first_text = _localizer["Subscribe_Paragraph_One"];
            var second_text = _localizer["Subscribe_Paragraph_Two"];
            var footer = _localizer["Subscribe_Footer"];

            var img_link = new Uri(new Uri(_mailSettings.Value.StaticFileServerAddress, UriKind.Absolute), "/static/MailLogo.png").ToString();
            //email
            var unsubsribe_link = new Uri(new Uri(_mailSettings.Value.WebServerAddress, UriKind.Absolute), $"/Home/Unsubscribe?email={email}").ToString();
            var unsubsribe_text = _localizer["Subscribe_Unsubscribe_Text"];


            var templateHtml = _localizer["Subscribe_Mail_Html", 
                title, 
                mailto_text, 
                view_browser, 
                topic, 
                first_text, 
                second_text, 
                footer, 
                img_link,
                email, 
                unsubsribe_link, 
                unsubsribe_text];

            var templateText = _localizer["Subscribe_Mail_Text",
                topic,
                first_text,
                second_text,
                footer,
                unsubsribe_link,
                unsubsribe_text];

            return SendEmailAsync(email, _localizer["Subscribe_Subject"], templateText, templateHtml);
        }
    }
}
