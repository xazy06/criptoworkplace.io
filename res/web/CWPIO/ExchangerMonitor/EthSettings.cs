using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangerMonitor
{
    public class EthSettings
    {
        public string NodeUrl { get; set; }
        public int Network { get; set; }
        public string AppAddress { get; set; }
        public string AppPrivateKey { get; set; }
        public string TokenContractAddr{ get; set; }
        public string SmartContractAddr { get; set; }
        public EthSettings()
        {
        }
    }
}
