using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Models
{
    public class EthSettings
    {
        public string NodeUrl { get; set; }
        public int Network { get; set; }
        public string AppAddress { get; set; }
        public string AppPK { get; set; }

        public EthSettings()
        {
        }
    }
}
