using ExchangerMonitor.Services;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ExchangerMonitor.WorkflowSteps
{
    public class CheckStatus : StepBodyAsync
    {
        public string Tx { get; set; }
        public TXStatus Status { get; set; }

        private readonly Eth _eth;
        public CheckStatus(Eth eth)
        {
            _eth = eth;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            ExchangeOperationStatus status = Status == TXStatus.Processed ? ExchangeOperationStatus.Skip : await _eth.GetTransactionStatus(Tx);
            return ExecutionResult.Outcome(status);
        }
    }
}
