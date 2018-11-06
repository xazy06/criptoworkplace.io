using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;

namespace pre_ico_web_site.Eth
{
    public class TransactionComparer : IEqualityComparer<Transaction>
    {
        public bool Equals(Transaction x, Transaction y)
        {
            return x.TransactionHash.Equals(y.TransactionHash);
        }

        public int GetHashCode(Transaction obj)
        {
            return obj.TransactionHash.GetHashCode();
        }
    }
}
