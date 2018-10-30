using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Models
{
    public class FailTransactionModel
    {
        public string Error { get; set; }
        public string Address { get; set; }
        public decimal IncomingCoin { get; set; }
        public string IncomingType { get; set; }
        public string Withdraw { get; set; }
    }
}
