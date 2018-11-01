using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Models
{
    public class MailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string StaticFileServerAddress { get; set; }
        public string WebServerAddress { get; set; }
        public Dictionary<string, int> ContacListId { get; set; }
        public Dictionary<string, int> WelcomeTemplateId { get; set; }
        public Dictionary<string, int> MailTemplateId { get; set; }

        public MailSettings()
        {
            ContacListId = new Dictionary<string, int>();
            WelcomeTemplateId = new Dictionary<string, int>();
        }
    }
}
