namespace ExchangerMonitor.Model
{
    public class MonitoringExchangeTransaction
    {
        public string Exchanger { get; set; }
        public string EthAmount { get; set; }
        public int Rate { get; set; }
        public int TokenCount { get; set; }
        public int FromBlock { get; set; }
    }
}

