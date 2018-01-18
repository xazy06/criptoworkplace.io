using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CWPIO.Services;

namespace CWPIO.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }

        public static Task SendEmailSubscription(this IEmailSender emailSender, string email, string name)
        {
            return emailSender.SendEmailAsync(email, "CWP Subscription", $"Hello, {name}. Info here");
        }
    }
}
