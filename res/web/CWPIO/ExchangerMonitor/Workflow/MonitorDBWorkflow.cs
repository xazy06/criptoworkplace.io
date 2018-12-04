using ExchangerMonitor.WorkflowSteps;
using System;
using WorkflowCore.Interface;

namespace ExchangerMonitor.Workflow
{
    public class MonitorDBWorkflow : IWorkflow
    {
        public string Id => "Monitor DB";

        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder.StartWith<LoadData>().Name("Load data")
                .Input(s => s.Period, d => TimeSpan.FromSeconds(10))
                .Input(s => s.BuyWorkflowName, d => "Buy Tokens")
                .OnError(WorkflowCore.Models.WorkflowErrorHandling.Retry, TimeSpan.FromSeconds(10));

        }
    }
}
