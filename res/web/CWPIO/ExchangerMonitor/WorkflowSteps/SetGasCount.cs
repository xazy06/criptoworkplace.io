using ExchangerMonitor.Model;
using ExchangerMonitor.Services;
using Microsoft.Extensions.Logging;
using System.Numerics;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ExchangerMonitor.WorkflowSteps
{
    public class SetGasCount : StepBodyAsync
    {
        private readonly IDatabaseService _db;
        private readonly ILogger _logger;
        private readonly IEthService _eth;

        public ExchangeTransaction Transaction { get; set; }

        public SetGasCount(IDatabaseService db, IEthService eth, ILogger<SendEth> logger)
        {
            _db = db;
            _logger = logger;
            _eth = eth;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            _logger.LogInformation("Set gas count for transaction {0}", Transaction.CurrentTx);
            var receipt = await _eth.GetTransactionReceiptAsync(Transaction.CurrentTx);

            if (receipt != null)
            {
                Transaction.TotalGasCount += (int)receipt.GasUsed.Value;
                await _db.SetTotalGasCount(Transaction.Id, Transaction.TotalGasCount);
            }            
            return ExecutionResult.Next();
        }
    }
}
