using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace pre_ico_web_site.Data
{
    public class ExchangeParameters
    {
        public string Exchanger { get; set; }
        public string EthAmount { get; set; }
        public int Rate { get; set; }
        public int TokenCount { get; set; }
        public int FromBlock { get; set; }
    }
}
