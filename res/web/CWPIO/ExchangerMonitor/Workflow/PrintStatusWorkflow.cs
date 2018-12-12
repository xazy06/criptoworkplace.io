using ExchangerMonitor.Model;
using ExchangerMonitor.WorkflowSteps;
using System;
using System.Collections.Generic;
using WorkflowCore.Interface;

namespace ExchangerMonitor.Workflow
{
    public class PrintStatusWorkflow : IWorkflow<Dictionary<string, ExchangeTransaction>>
    {
        public string Id => "Print Status";

        public int Version => 1;

        public void Build(IWorkflowBuilder<Dictionary<string, ExchangeTransaction>> builder)
        {
            builder.StartWith<PrintData>().Name("Load data")
                .Input(s => s.Period, d => TimeSpan.FromSeconds(10))
                .Input(s => s.MonitoredData, d => d)
                .OnError(WorkflowCore.Models.WorkflowErrorHandling.Retry, TimeSpan.FromSeconds(10));
        }
    }
}
