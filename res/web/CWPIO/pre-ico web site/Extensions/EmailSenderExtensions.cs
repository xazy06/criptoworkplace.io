using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace pre_ico_web_site.Services
{
    public static class EmailSenderExtensions
    {
        public static Task<bool> SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link, string header, string footer)
        {
            var html = header + $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>" + footer;

            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by following this link: '{HtmlEncoder.Default.Encode(link)}'",
                html);
                //$"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }        
    }
}
