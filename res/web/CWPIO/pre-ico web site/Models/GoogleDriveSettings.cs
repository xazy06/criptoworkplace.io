using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Models
{
    public class GoogleDriveSettings
    {
        public string ServiceAccount { get; set; }
        public string P12CertPath { get; set; }
        public string SaleContractFileName { get; set; }
        public string TokenContractFileName { get; set; }
        public string WPFileName { get; set; }
        public string OnePagerFileName { get; set; }
    }
}
