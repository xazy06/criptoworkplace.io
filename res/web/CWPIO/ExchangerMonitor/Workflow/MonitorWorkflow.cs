using ExchangerMonitor.WorkflowSteps;
using System;
using WorkflowCore.Interface;

namespace ExchangerMonitor.Workflow
{
    public class MonitorWorkflow : IWorkflow
    {
        public string Id => "Monitor";

        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder.StartWith<CustomMessage>().Name("Load data message")
                .Input(s => s.Message, d=> "Start monitoring transactions on exchanger")
                .Then<LoadMonitorData>(s => s.Delay(d => TimeSpan.FromSeconds(30)).Then(s))
                .OnError(WorkflowCore.Models.WorkflowErrorHandling.Retry, TimeSpan.FromSeconds(10));

        }
    }
}
