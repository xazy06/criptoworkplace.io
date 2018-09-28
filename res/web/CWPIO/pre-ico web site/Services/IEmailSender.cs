using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Services
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string email, string subject, string message, string html = null);
        Task<bool> SendEmailSubscription(string email, string name);
    }
}
