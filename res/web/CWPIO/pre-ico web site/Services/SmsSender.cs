using pre_ico_web_site.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Services
{
    public class SmsSender:ISmsSender
    {
        public SMSoptions Options { get; }  // set only via Secret Manager

        public SmsSender(IOptions<SMSoptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public Task SendSmsAsync(string number, string message)
        {
            //ASPSMS.SMS SMSSender = new ASPSMS.SMS();

            //SMSSender.Userkey = Options.SMSAccountIdentification;
            //SMSSender.Password = Options.SMSAccountPassword;
            //SMSSender.Originator = Options.SMSAccountFrom;

            //SMSSender.AddRecipient(number);
            //SMSSender.MessageData = message;

            //SMSSender.SendTextSMS();

            return Task.FromResult(0);
        }
    }
}
