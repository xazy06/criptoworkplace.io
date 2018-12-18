using ExchangerMonitor.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ExchangerMonitor.WorkflowSteps
{
    public class LoadMonitorData : StepBodyAsync
    {
        private readonly IDatabaseService _db;
        private readonly IEthService _eth;
        private readonly ILogger _logger;

        public LoadMonitorData(IDatabaseService db, ILogger<LoadData> logger, IEthService eth)
        {
            _db = db;
            _logger = logger;
            _eth = eth;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            _logger.LogDebug("Checking db monitor data...");
            var res = await _db.GetMonitoringExchangeAsync();
            _logger.LogInformation("Found {0} items", res.Count);
            List<Task> tasks = new List<Task>();
            foreach (var r in res)
            {
                var t = Task.Run(async () =>
                {
                    _logger.LogDebug("Search transaction for {0} from {1}", r.Exchanger, r.FromBlock);
                    var (tx, blockNumber) = await _eth.GetInTransactionFromBlockAsync(r.Exchanger, r.FromBlock);

                    if (string.IsNullOrEmpty(tx))
                    {
                        _logger.LogDebug("Not found. Update from block {0} {1}", r.Exchanger, blockNumber);
                        await _db.UpdateFromBlockAsync(r.Exchanger, blockNumber);
                    }
                    else
                    {
                        _logger.LogInformation("Start transaction for {0}", r.Exchanger);
                        await _db.StartTransactionAsync(tx, r.Rate, r.Exchanger, r.EthAmount, r.TokenCount);
                    }
                });
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());
            return ExecutionResult.Next();
        }
    }
}
