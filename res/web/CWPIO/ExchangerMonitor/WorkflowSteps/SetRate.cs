using ExchangerMonitor.Model;
using ExchangerMonitor.Services;
using Microsoft.Extensions.Logging;
using System.Numerics;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ExchangerMonitor.WorkflowSteps
{
    public class SetRate : StepBodyAsync
    {
        private readonly IDatabaseService _db;
        private readonly ILogger _logger;
        private readonly IEthService _eth;

        public ExchangeTransaction Transaction { get; set; }

        public SetRate(IDatabaseService db, IEthService eth, ILogger<SetRate> logger)
        {
            _db = db;
            _logger = logger;
            _eth = eth;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            _logger.LogInformation("Set fix rate");
            var amount = BigInteger.Parse(Transaction.EthAmount);
            var tx = await _eth.SetRateAsync(Transaction.Rate, Transaction.ETHAddress, amount);

            Transaction.CurrentTx = tx;
            Transaction.CurrentStep = (int)ChangeSteps.SendEth;
            await _db.SetCurrentTransaction(Transaction.Id, tx);
            await _db.SetStep(Transaction.Id, (int)ChangeSteps.SendEth);

            return ExecutionResult.Next();
        }
    }
}
