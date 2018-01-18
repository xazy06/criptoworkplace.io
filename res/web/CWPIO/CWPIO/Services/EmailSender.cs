using CWPIO.Models;
using Microsoft.Extensions.Options;
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
        SmtpClient _client;
        public EmailSender(IOptions<MailSettings> mailSettings)
        {
            _client = new SmtpClient(mailSettings.Value.Host, mailSettings.Value.Port)
            {
                Credentials = new System.Net.NetworkCredential(mailSettings.Value.UserName, mailSettings.Value.Password),
                EnableSsl = true
            };
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return _client.SendMailAsync("robot@cryptoworkplace.io", email, subject, message);
        }
    }
}
