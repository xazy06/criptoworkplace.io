using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangerMonitor.Model;

namespace ExchangerMonitor.Services
{
    public interface IDatabaseService
    {
        Task<List<ExchangeTransaction>> GetActiveExchangeTransactionsAsync();
        Task<string> GetAddressExchangerAsync(string addr);
        Task MarkAsEnded(string id);
        Task MarkAsFailed(string id);
        Task SetCurrentTransaction(string id, string transaction);
        Task SetStep(string id, int step);
        Task SetTotalGasCount(string id, int gas);
        Task<List<MonitoringExchangeTransaction>> GetMonitoringExchangeAsync();
        Task StartTransactionAsync(string tx, int rate, string exchanger, string ethAmount, int tokenCount);
        Task UpdateFromBlockAsync(string exchanger, int blockNumber);
    }
}