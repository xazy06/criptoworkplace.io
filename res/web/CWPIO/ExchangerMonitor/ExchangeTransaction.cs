using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangerMonitor
{
    public class ExchangeTransaction
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ETHAddress { get; set; }
        public string StartTx { get; set; }
        public string CurrentTx { get; set; }
        public string RefundTx { get; set; }
        public string EthAmount { get; set; }
        public string TokenAmount { get; set; }
        public TXStatus Status { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}\tStartTx: {StartTx}\tCurrentTx: {CurrentTx}";
        }
    }
    
    public enum TXStatus
    {
        Ok = 0,
        Failed = 1,
        Ended = 2
    }
}
