using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;


namespace ExchangerMonitor.Model
{
    [FunctionOutput]
    public class FixRateModel
    {
        [Parameter("uint256", "rate", 1)]
        public int Rate { get; set; }

        [Parameter("uint256", "time", 2)]
        public long Time { get; set; }

        [Parameter("uint256", "amount", 3)]
        public BigInteger Amount { get; set; }
    }
}