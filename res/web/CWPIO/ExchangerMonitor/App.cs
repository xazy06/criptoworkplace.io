using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangerMonitor
{
    public class App
    {
        private Database _db;
        private Eth _eth;

        private readonly Dictionary<string, ExchangeTransaction> _monitored = new Dictionary<string, ExchangeTransaction>();
        private readonly List<ConsoleLogEntry> _actionLogs = new List<ConsoleLogEntry>(10);
        private Timer _checkDbTimer;
        private Timer _printDataTimer;

        public async Task Run()
        {
            _db = new Database(Environment.GetEnvironmentVariable("ConnectionStrings:CWPConnection"));
            _eth = new Eth("https://ropsten.infura.io/roht23j583p4SPym7gx6", this);
            //_eth = new Eth("http://104.209.40.23:8545/", this);

            var res = await _db.GetActiveExchangeTransactionsAsync();
            lock (_monitored)
            {
                res.ForEach(item => _monitored[item.StartTx] = item);
            }

            PrintCurrentMon();
            _checkDbTimer = new Timer(new TimerCallback(async (o) =>
              {
                  lock (_monitored)
                  {
                      Parallel.ForEach(_monitored.Keys, async item => _monitored[item] = await ProcessExchangeItemAsync(_monitored[item]));
                  }

                  await CheckDbAsync();
              }), null, TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(1));
            _printDataTimer = new Timer(new TimerCallback((_) => { PrintCurrentMon(); }), null, 0, 5000);
        }

        private void PrintCurrentMon()
        {

            Console.Clear();
            Console.WriteLine("Current monitored:");
            Console.WriteLine("------------------------------------------------");
            lock (_monitored)
            {
                foreach (var item in _monitored)
                {
                    Console.WriteLine(item.Value);
                }
            }

            Console.WriteLine("------------------------------------------------");
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

        public void AddToLog(string log, ConsoleColor color = ConsoleColor.White)
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

        private async Task CheckDbAsync()
        {
            var res = await _db.GetActiveExchangeTransactionsAsync();
            lock (_monitored)
            {
                res.ForEach(item => _monitored[item.StartTx] = item);
            }

            PrintCurrentMon();

            lock (_monitored)
            {
                var toRemove = _monitored.Where(pair => pair.Value.Status == TXStatus.Ended || pair.Value.Status == TXStatus.Failed)
                         .Select(pair => pair.Key)
                         .ToList();
                foreach (var key in toRemove)
                {
                    _monitored.Remove(key);
                }
            }


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
            return item;
        }

        private async Task NextStage(ExchangeTransaction item)
        {
            try
            {
                var currentTo = await _eth.GetTransactionToAsync(item.CurrentTx);
                if (item.StartTx == item.CurrentTx)
                {
                    //var amount = await _eth.GetTransactionAmount(item.StartTx);
                    var amount = new HexBigInteger(BigInteger.Parse(item.EthAmount));
                    AddToLog($"send {amount.Value} ETH to contract");
                    var newTx = await _eth.SendToSmartContractAsync(amount);
                    item.CurrentTx = newTx;
                    await _db.SetCurrentTransaction(item.Id, newTx);
                }
                else if (currentTo == item.ETHAddress)
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
