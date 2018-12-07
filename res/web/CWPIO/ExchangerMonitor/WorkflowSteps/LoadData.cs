using ExchangerMonitor.Model;
using ExchangerMonitor.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ExchangerMonitor.WorkflowSteps
{
    public class LoadData : StepBodyAsync
    {
        public TimeSpan Period { get; set; }
        public string BuyWorkflowName { get; set; }

        private readonly IDatabaseService _db;

        private readonly ILogger _logger;
        private readonly IWorkflowHost _host;

        public LoadData(IDatabaseService db, ILogger<LoadData> logger, IWorkflowHost host)
        {
            _db = db;
            _logger = logger;
            _host = host;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            _logger.LogDebug("Checking db...");
            var res = await _db.GetActiveExchangeTransactionsAsync();

            var monitored = context.PersistenceData as Dictionary<string, ExchangeTransaction> ?? new Dictionary<string, ExchangeTransaction>();
            if (context.PersistenceData == null)
                await _host.StartWorkflow("Print Status", monitored);

            //Parallel.ForEach(monitored, pair =>
            //{
            //    if (pair.Value.Status == TXStatus.Ended || pair.Value.Status == TXStatus.Failed)
            //    {
            //        monitored.Remove(pair.Key);
            //    }
            //});
            
            Parallel.ForEach(res, async item =>
            {
                if (!monitored.ContainsKey(item.StartTx))
                {
                    monitored[item.StartTx] = item;
                    var t = await _host.StartWorkflow(BuyWorkflowName, monitored[item.StartTx], monitored[item.StartTx].StartTx);
                    _logger.LogInformation($"Starting workflow: {t}");
                    monitored[item.StartTx].Workflow = t;
                }
            });

            var toRemove = monitored.Where(pair => pair.Value.Status == TXStatus.Ended || pair.Value.Status == TXStatus.Failed)
                        .Select(pair => pair.Key)
                        .ToList();
            _logger.LogDebug($"Remove {toRemove.Count} items");
            foreach (var key in toRemove)
            {
                monitored.Remove(key);
            }

            _logger.LogDebug("DB checked...");
            return ExecutionResult.Sleep(Period, monitored);
        }
    }
}
