using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ExchangerMonitor.WorkflowSteps
{
    public class CustomMessage : StepBody
    {
        private ILogger _logger { get; set; }
        public CustomMessage(ILogger<CustomMessage> logger)
        {
            _logger = logger;
        }
        public string Message { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation(Message);
            return ExecutionResult.Next();
        }
    }
}
