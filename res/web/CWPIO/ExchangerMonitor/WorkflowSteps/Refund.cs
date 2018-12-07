using ExchangerMonitor.Model;
using ExchangerMonitor.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ExchangerMonitor.WorkflowSteps
{
    public class Refund : StepBodyAsync
    {
        private readonly IDatabaseService _db;
        private readonly ILogger _logger;
        private readonly IEthService _eth;

        public ExchangeTransaction Transaction { get; set; }

        public Refund(IDatabaseService db, IEthService eth, ILogger<Refund> logger)
        {
            _db = db;
            _logger = logger;
            _eth = eth;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            _logger.LogInformation("Send eth to customer");
            var exchanger = await _db.GetAddressExchangerAsync(Transaction.TempAddress);
            var tx = await _eth.SendRefundToUserAsync(exchanger, Transaction.ETHAddress);

            Transaction.CurrentTx = tx;
            Transaction.CurrentStep = (int)ChangeSteps.Finish;
            await _db.SetCurrentTransaction(Transaction.Id, tx);
            await _db.SetStep(Transaction.Id, (int)ChangeSteps.Finish);

            return ExecutionResult.Next();
        }
    }
}
