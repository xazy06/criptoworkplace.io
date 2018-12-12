using ExchangerMonitor.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ExchangerMonitor.WorkflowSteps
{
    public class PrintData : StepBody
    {
        public TimeSpan Period { get; set; }
        public Dictionary<string, ExchangeTransaction> MonitoredData { get; set; }


        private readonly ILogger _logger;

        public PrintData(ILogger<LoadData> logger)
        {
            _logger = logger;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var txt = new StringBuilder();
            txt.AppendLine("Current monitored:");
            txt.AppendLine("------------------------------------------------");
            foreach (var item in MonitoredData)
            {
                txt.AppendLine(item.Value.ToString());
            }
            txt.AppendLine("------------------------------------------------");

            _logger.LogInformation(txt.ToString());
            return ExecutionResult.Sleep(Period, null);
        }
    }
}
