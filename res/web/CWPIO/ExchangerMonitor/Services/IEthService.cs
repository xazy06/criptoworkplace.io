using System.Numerics;
using System.Threading.Tasks;
using ExchangerMonitor.Model;
using Nethereum.RPC.Eth.DTOs;

namespace ExchangerMonitor.Services
{
    public interface IEthService
    {
        Task<FixRateModel> GetRateForBuyerAsync(string buyer);
        Task<TransactionReceipt> GetTransactionReceiptAsync(string transactionHash);
        Task<ExchangeOperationStatus> GetTransactionStatus(string txHash);
        Task<string> SendRefundToUserAsync(string pk, string to);
        Task<string> SendToSmartContractAsync(string privateKey, string buyer, BigInteger amount);
        Task<string> SetRateAsync(int rate, string buyer, BigInteger amount);
        Task<(string tx, int blockNumber)> GetInTransactionFromBlockAsync(string exchanger, int fromBlock);
    }
}