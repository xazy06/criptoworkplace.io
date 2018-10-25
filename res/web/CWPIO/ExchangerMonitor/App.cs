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
        private bool _inContainer;
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

        public async Task Run(bool inContainer)
        {
            _inContainer = inContainer;
            var res = await _db.GetActiveExchangeTransactionsAsync();
            lock (_monitored)
            {
                res.ForEach(item => _monitored[item.StartTx] = item);
            }

            _checkDbTimer = new Timer(new TimerCallback(async (o) =>
              {
                  lock (_monitored)
                  {
                      Parallel.ForEach(_monitored.Keys, async item => _monitored[item] = await ProcessExchangeItemAsync(_monitored[item]));
                  }

                  await CheckDbAsync();
              }), null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(30));
            _printDataTimer = new Timer(new TimerCallback((_) => { PrintCurrentMon(); }), null, 0, _inContainer ? 30000 : 5000);
        }

        private void PrintCurrentMon()
        {
            if (!_inContainer)
            {
                Console.Clear();
            }

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


            if (!_inContainer)
            {
                lock (_actionLogs)
                {
                    foreach (var item in _actionLogs)
                    {
                        var tmpColor = Console.ForegroundColor;
                        Console.ForegroundColor = item.Color;
                        Console.WriteLine(item.Message);
                        Console.ForegroundColor = tmpColor;
                    }
                }
            }
        }

        public void AddToLog(string log, ConsoleColor color = ConsoleColor.White)
        {

            if (!_inContainer)
            {
                lock (_actionLogs)
                {
                    if (_actionLogs.Count == 10)
                    {
                        _actionLogs.RemoveAt(0);
                    }

                    _actionLogs.Add(new ConsoleLogEntry { Message = log, Color = color });
                }
            }
            else
            {
                _logger.LogDebug(log);
            }

        }

        private async Task CheckDbAsync()
        {
            AddToLog("Checking db...");
            var res = await _db.GetActiveExchangeTransactionsAsync();
            lock (_monitored)
            {
                res.ForEach(item => _monitored[item.StartTx] = item);

                var toRemove = _monitored.Where(pair => pair.Value.Status == TXStatus.Ended || pair.Value.Status == TXStatus.Failed)
                         .Select(pair => pair.Key)
                         .ToList();
                AddToLog($"Remove {toRemove.Count} items");
                foreach (var key in toRemove)
                {
                    _monitored.Remove(key);
                }
            }
            AddToLog("DB checked...");
            PrintCurrentMon();
        }

        private async Task<ExchangeTransaction> ProcessExchangeItemAsync(ExchangeTransaction item)
        {
            ConsoleColor selectColor(ExchangeOperationStatus s) =>
                s == ExchangeOperationStatus.Ok ? ConsoleColor.Green :
                    (s == ExchangeOperationStatus.Failed ? ConsoleColor.Red : ConsoleColor.Yellow);

            try
            {
                ExchangeOperationStatus status = await _eth.GetTransactionStatus(item.CurrentTx);


                AddToLog($"Tx: {item.CurrentTx} Status: {status}", selectColor(status));
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

                AddToLog(ex.ToString());
            }

            //if (!string.IsNullOrEmpty(item.RefundTx))

            return item;
        }

        private async Task NextStage(ExchangeTransaction item)
        {
            try
            {
                var transaction = await _eth.GetTransactionToAsync(item.CurrentTx);
                if (item.StartTx == item.CurrentTx)
                {
                    //var amount = await _eth.GetTransactionAmount(item.StartTx);
                    var amount = new HexBigInteger(BigInteger.Parse(item.EthAmount));
                    AddToLog($"send {amount.Value} ETH to contract");
                    var (tx, gasPrice) = await _eth.SendToSmartContractAsync(amount);
                    item.CurrentTx = tx;
                    await _db.SetCurrentTransaction(item.Id, tx);

                    var toRefund = transaction.Value.Value - amount.Value - gasPrice * (95000 + 21000);
                    if (toRefund > 0)
                    {
                        AddToLog($"refund: {toRefund}");
                        
                        
                        var refundTx = await _eth.SendRefundToUserAsync(item.ETHAddress, new HexBigInteger(toRefund));
                        await _db.SetRefundTransaction(item.Id, refundTx);
                    }
                }
                else if (transaction.To == item.ETHAddress)
                {
                    item.Status = TXStatus.Ended;
                    await _db.MarkAsEnded(item.Id);
                }
                else
                {
                    //var tokens = await _eth.GetTokensFromTransaction(item.CurrentTx);
                    var tokens = new HexBigInteger(BigInteger.Parse(item.TokenAmount));
                    AddToLog($"send {tokens.Value} CWT-P to user");
                    var newTx = await _eth.SendToUserAsync(item.ETHAddress, tokens);
                    item.CurrentTx = newTx;
                    await _db.SetCurrentTransaction(item.Id, newTx);
                }
                AddToLog("move to next transaction");
            }
            catch (RpcResponseException rpce)
            {
                if (rpce.RpcError.Message.Contains("insufficient funds"))
                {
                    AddToLog("NEED TO PAY!!!!!!", ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                AddToLog(ex.ToString());
            }
        }
    }

    internal class ConsoleLogEntry
    {
        public string Message { get; set; }
        public ConsoleColor Color { get; set; }
    }
}
