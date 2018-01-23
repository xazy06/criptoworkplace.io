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
        public static Task<bool> SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email", 
                $"Please confirm your account by following this link: '{HtmlEncoder.Default.Encode(link)}'",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }

        public static Task<bool> SendEmailSubscription(this IEmailSender emailSender, string email, string name)
        {
            return emailSender.SendEmailAsync(email, "CWP Subscription", $"Hello, {name}. Info here", $"<h1>Html here</h1>");
        }
    }
}
