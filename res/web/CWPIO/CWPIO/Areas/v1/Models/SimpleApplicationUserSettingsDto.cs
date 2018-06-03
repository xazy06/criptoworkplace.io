using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Areas.v1.Models
{
    public class SimpleApplicationUserSettingsDto
    {
        public string EthAddress { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string TelegramNickname { get; set; }
    }
}
