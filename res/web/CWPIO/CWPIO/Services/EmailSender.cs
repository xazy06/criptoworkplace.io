using CWPIO.Models;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private ILogger _logger;
        public EmailSender(IOptions<MailSettings> mailSettings, IStringLocalizer<EmailSender> localizer, ILogger<EmailSender> logger)
        {
            _client = new MailjetClient(mailSettings.Value.ApiKey, mailSettings.Value.ApiSecret) { Version = ApiVersion.V3 };
            _localizer = localizer;
            _mailSettings = mailSettings;
            _logger = logger;
        }


        public async Task<bool> SendEmailAsync(string email, string subject, string message, string html = null)
        {

            MailjetRequest request = new MailjetRequest { Resource = Send.Resource }
                .Property(Send.FromEmail, "info@cryptoworkplace.io")
                .Property(Send.FromName, "CryptoWorkPlace Info")
                .Property(Send.Subject, subject)
                .Property(Send.TextPart, message);
            if (!string.IsNullOrEmpty(html))
                request.Property(Send.HtmlPart, html);

            request.Property(Send.Recipients, new JArray { new JObject { { "Email", email } } });


            MailjetResponse response = await _client.PostAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SendEmailSubscription(string email, string name)
        {
            var res = await AddUserToContactList(email, name);
            if (res)
            {
                MailjetRequest request = new MailjetRequest { Resource = Send.Resource }
                    .Property(Send.FromEmail, "info@cryptoworkplace.io")
                    .Property(Send.FromName, "CryptoWorkPlace Info")
                    .Property(Send.Subject, _localizer["Subscribe_Subject"].Value)
                    .Property(Send.MjTemplateID, _mailSettings.Value.WelcomeTemplateId[CultureInfo.CurrentUICulture.Name])
                    .Property(Send.MjTemplateLanguage, true)
                    .Property(Send.Vars, new JObject {
                    { "header", _localizer["Subscribe_Topic"].Value },
                    { "text1", _localizer["Subscribe_Paragraph_One"].Value },
                    { "text2", _localizer["Subscribe_Paragraph_Two"].Value },
                    { "text3", _localizer["Subscribe_Paragraph_Three"].Value }
                    })
                    .Property(Send.Recipients, new JArray { new JObject { { "Email", email } } });

                MailjetResponse response = await _client.PostAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    _logger.LogError(response.GetErrorMessage());

            }
            return false;

            /*var title = _localizer["Subscribe_Title"];
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
            var third_text = _localizer["Subscribe_Paragraph_Three"];

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
                unsubsribe_text,
                third_text];

            var templateText = _localizer["Subscribe_Mail_Text",
                topic,
                first_text,
                second_text,
                footer,
                unsubsribe_link,
                unsubsribe_text,
                third_text];

            return SendEmailAsync(email, _localizer["Subscribe_Subject"], templateText, templateHtml);*/
        }

        private async Task<bool> AddUserToContactList(string email, string name)
        {
            var request = new MailjetRequest { Resource = Contact.Resource, ResourceId = ResourceId.Alphanumeric(email) };
            var response = await _client.GetAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                request = new MailjetRequest { Resource = Contact.Resource }
                    .Property(Contact.Name, name)
                    .Property(Contact.Email, email);
                response = await _client.PostAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    request = new MailjetRequest { Resource = Contact.Resource, ResourceId = ResourceId.Alphanumeric(email) };
                    response = await _client.GetAsync(request);
                }
            }

            if (response.IsSuccessStatusCode)
            {
                var contactId = response.GetData().First.Value<long>(Contact.ID);

                request = new MailjetRequest { Resource = Contactdata.Resource, ResourceId = ResourceId.Numeric(contactId) }
                    .Property(Contactdata.Data, new JArray {
                        new JObject { {"Name", "country" }, { "Value", CultureInfo.CurrentUICulture.Name } }
                    });
                response = await _client.PutAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    request = new MailjetRequest { Resource = ContactManagecontactslists.Resource, ResourceId = ResourceId.Numeric(contactId) }
                        .Property(ContactManagecontactslists.ContactsLists, new JArray {
                            new JObject {
                                { "ListId", _mailSettings.Value.ContacListId[CultureInfo.CurrentUICulture.Name].ToString() },
                                { "Action", "addnoforce" }
                            }
                        });
                    response = await _client.PostAsync(request);
                    if (response.IsSuccessStatusCode)
                        return true;
                    else
                        _logger.LogError(response.GetErrorMessage());
                }
                else
                    _logger.LogError(response.GetErrorMessage());
            }
            else
                _logger.LogError(response.GetErrorMessage());
            return false;
        }

    }
}
