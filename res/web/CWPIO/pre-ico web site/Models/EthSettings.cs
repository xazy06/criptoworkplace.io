using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Models
{
    public class EthSettings
    {
        public string NodeUrl { get; set; }
        public int Network { get; set; }
        public string AppAddress { get; set; }
        public string AppPK { get; set; }
        public decimal TokenPrice { get; set; }
        public long GasLimit { get; set; }
        public int GasPrice { get; set; }
        public EthSettings()
        {
        }
    }
}
