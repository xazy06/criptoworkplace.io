using Microsoft.Extensions.Logging;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangerMonitor
{
    public class App
    {
        private readonly Database _db;
        private readonly Eth _eth;
        private readonly ILogger _logger;
        private readonly Dictionary<string, ExchangeTransaction> _monitored = new Dictionary<string, ExchangeTransaction>();
        private readonly List<ConsoleLogEntry> _actionLogs = new List<ConsoleLogEntry>(10);
        private Timer _checkDbTimer;
        private Timer _printDataTimer;

        public App(Eth eth, Database db, ILogger<App> logger)
        {
            _db = db;
            _eth = eth;
            _logger = logger;
        }

        public void Run()
        {
            _checkDbTimer = new Timer(new TimerCallback(async (o) =>
              {
                  await CheckDbAsync();
                  lock (_monitored)
                  {
                      Parallel.ForEach(_monitored.Keys, async item => _monitored[item] = await ProcessExchangeItemAsync(_monitored[item]));
                  }
              }), null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(30));
            _printDataTimer = new Timer(new TimerCallback((_) => { PrintCurrentMon(); }), null, 0, 10000);
        }

        private void PrintCurrentMon()
        {
            var txt = new StringBuilder();
            txt.AppendLine("Current monitored:");
            txt.AppendLine("------------------------------------------------");
            lock (_monitored)
            {
                foreach (var item in _monitored)
                {
                    txt.AppendLine(item.Value.ToString());
                }
            }
            txt.AppendLine("------------------------------------------------");
            _logger.LogInformation(txt.ToString());
        }

        private async Task CheckDbAsync()
        {
            _logger.LogDebug("Checking db...");
            var res = await _db.GetActiveExchangeTransactionsAsync();
            lock (_monitored)
            {
                res.ForEach(item => _monitored[item.StartTx] = item);

                var toRemove = _monitored.Where(pair => pair.Value.Status == TXStatus.Ended || pair.Value.Status == TXStatus.Failed)
                         .Select(pair => pair.Key)
                         .ToList();
                _logger.LogDebug($"Remove {toRemove.Count} items");
                foreach (var key in toRemove)
                {
                    _monitored.Remove(key);
                }
            }
            _logger.LogDebug("DB checked...");
            PrintCurrentMon();
        }

        private async Task<ExchangeTransaction> ProcessExchangeItemAsync(ExchangeTransaction item)
        {
            try
            {
                ExchangeOperationStatus status = await _eth.GetTransactionStatus(item.CurrentTx);
                
                _logger.LogDebug($"Tx: {item.CurrentTx} Status: {status}");
                switch (status)
                {
                    case ExchangeOperationStatus.Ok:
                        await NextStage(item);
                        break;

                    case ExchangeOperationStatus.Skip:
                        break;
                    case ExchangeOperationStatus.Failed:
                        item.Status = TXStatus.Failed;
                        await _db.MarkAsFailed(item.Id);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.ToString());
            }
            
            return item;
        }

        private async Task NextStage(ExchangeTransaction item)
        {
            try
            {
                var transaction = await _eth.GetTransactionToAsync(item.CurrentTx);
                if (item.StartTx == item.CurrentTx)
                {
                    var amount = new HexBigInteger(BigInteger.Parse(item.EthAmount));
                    _logger.LogDebug($"send {amount.Value} ETH to contract");

                    var (tx, gasPrice) = await _eth.SendToSmartContractAsync(amount, transaction.To);
                    item.CurrentTx = tx;
                    await _db.SetCurrentTransaction(item.Id, tx);

                    //var toRefund = transaction.Value.Value - amount.Value - gasPrice * (95000 + 21000);
                    //if (toRefund > 0)
                    //{
                    //    _logger.LogDebug($"refund: {toRefund}");
                        
                        
                    //    var refundTx = await _eth.SendRefundToUserAsync(item.ETHAddress, new HexBigInteger(toRefund));
                    //    await _db.SetRefundTransaction(item.Id, refundTx);
                    //}
                }
                else// if (transaction.To == item.ETHAddress)
                {
                    item.Status = TXStatus.Ended;
                    await _db.MarkAsEnded(item.Id);
                }
                //else
                //{
                //    //var tokens = await _eth.GetTokensFromTransaction(item.CurrentTx);
                //    var tokens = new HexBigInteger(BigInteger.Parse(item.TokenAmount));
                //    _logger.LogDebug($"send {tokens.Value} CWT-P to user");
                //    var newTx = await _eth.SendToUserAsync(item.ETHAddress, tokens);
                //    item.CurrentTx = newTx;
                //    await _db.SetCurrentTransaction(item.Id, newTx);
                //}
                _logger.LogDebug("move to next transaction");
            }
            catch (RpcResponseException rpce)
            {
                if (rpce.RpcError.Message.Contains("insufficient funds"))
                {
                    _logger.LogDebug("NEED TO PAY!!!!!!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.ToString());
            }
        }
    }

    internal class ConsoleLogEntry
    {
        public string Message { get; set; }
        public ConsoleColor Color { get; set; }
    }
}
