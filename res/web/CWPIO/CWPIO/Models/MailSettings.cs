using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Models
{
    public class MailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string StaticFileServerAddress { get; set; }
        public string WebServerAddress { get; set; }
    }
}
