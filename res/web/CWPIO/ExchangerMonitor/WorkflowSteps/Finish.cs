using ExchangerMonitor.Model;
using ExchangerMonitor.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ExchangerMonitor.WorkflowSteps
{
    public class Finish : StepBodyAsync
    {
        private readonly IDatabaseService _db;
        private readonly ILogger _logger;

        public ExchangeTransaction Transaction { get; set; }

        public Finish(IDatabaseService db, ILogger<Finish> logger)
        {
            _db = db;
            _logger = logger;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            _logger.LogInformation("Finish");

            Transaction.Status = TXStatus.Ended;
            await _db.MarkAsEnded(Transaction.Id);

            return ExecutionResult.Next();
        }
    }
}
