using ExchangerMonitor.Model;
using ExchangerMonitor.Services;
using Microsoft.Extensions.Logging;
using System.Numerics;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ExchangerMonitor.WorkflowSteps
{
    public class SendEth : StepBodyAsync
    {
        private readonly IDatabaseService _db;
        private readonly ILogger _logger;
        private readonly IEthService _eth;

        public ExchangeTransaction Transaction { get; set; }

        public SendEth(IDatabaseService db, IEthService eth, ILogger<SendEth> logger)
        {
            _db = db;
            _logger = logger;
            _eth = eth;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            _logger.LogInformation("Send eth to exchanger contract");
            var exchanger = await _db.GetAddressExchangerAsync(Transaction.TempAddress);
            string tx = await _eth.SendToSmartContractAsync(exchanger, Transaction.ETHAddress, BigInteger.Parse(Transaction.EthAmount));

            Transaction.CurrentTx = tx;
            Transaction.CurrentStep = (int)ChangeSteps.Refund;

            await _db.SetCurrentTransaction(Transaction.Id, tx);
            await _db.SetStep(Transaction.Id, (int)ChangeSteps.Refund);

            return ExecutionResult.Next();
        }
    }
}
