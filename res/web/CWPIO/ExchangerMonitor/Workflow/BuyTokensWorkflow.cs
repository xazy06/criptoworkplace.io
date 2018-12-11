using ExchangerMonitor.Model;
using ExchangerMonitor.WorkflowSteps;
using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ExchangerMonitor.Workflow
{
    public partial class BuyTokensWorkflow : IWorkflow<ExchangeTransaction>
    {
        public string Id => "Buy Tokens";

        public int Version => 1;

        public void Build(IWorkflowBuilder<ExchangeTransaction> builder)
        {
            void cfgMessage(IStepBuilder<ExchangeTransaction, CustomMessage> c) => c.Name("Message").Input(step => step.Message, data => "Check status running: " + data.CurrentTx);

            builder.StartWith<CustomMessage>(cfgMessage)
                .Saga(saga =>
                    saga
                    .StartWith<CustomMessage>(cfgMessage)
                    .Then<CheckStatus>(checkStatusStep =>
                    {
                        checkStatusStep
                            .Name("Check status")
                            .Input(s => s.Status, d => d.Status)
                            .Input(s => s.Tx, d => d.CurrentTx);

                        checkStatusStep
                            .When(d => ExchangeOperationStatus.Ok, "Status OK")
                            .Do(doBuilder =>
                            {
                                doBuilder
                                    .StartWith<CustomMessage>(s => s.Name("Ok Message").Input(step => step.Message, data => "OK transaction: " + data.CurrentTx))
                                    .Then<SetGasCount>(c => c.Name("Set gas count").Input(step => step.Transaction, data => data))
                                    .If(d => (ChangeSteps)d.CurrentStep == ChangeSteps.SetRate).Do(sb =>
                                    {
                                        sb.StartWith<SetRate>(s => s.Name("Set Rate").Input(step => step.Transaction, data => data))
                                        .Schedule(d => TimeSpan.FromSeconds(10))
                                        .Do(isb => isb.StartWith<CustomMessage>(cfgMessage).Then(checkStatusStep));
                                    })
                                    .If(d => (ChangeSteps)d.CurrentStep == ChangeSteps.SendEth).Do(sb =>
                                    {
                                        sb.StartWith<SendEth>(s => s.Name("Send Eth").Input(step => step.Transaction, data => data))
                                        .Schedule(d => TimeSpan.FromSeconds(10))
                                        .Do(isb => isb.StartWith<CustomMessage>(cfgMessage).Then(checkStatusStep));
                                    })
                                    .If(d => (ChangeSteps)d.CurrentStep == ChangeSteps.Refund).Do(sb =>
                                    {
                                        sb.StartWith<Refund>(s => s.Name("Refund").Input(step => step.Transaction, data => data))
                                        .Schedule(d => TimeSpan.FromSeconds(10))
                                        .Do(isb => isb.StartWith<CustomMessage>(cfgMessage).Then(checkStatusStep));
                                    })
                                    .If(d => (ChangeSteps)d.CurrentStep == ChangeSteps.Finish && d.Status != TXStatus.Failed).Do(sb =>
                                    {
                                        sb.StartWith<Finish>(s => s.Name("Finish").Input(step => step.Transaction, data => data))
                                        .EndWorkflow();
                                    });
                            });

                        checkStatusStep
                            .When(d => ExchangeOperationStatus.Skip, "Status Skip")
                            .Do(doBuilder =>
                            {
                                doBuilder
                                    .StartWith<CustomMessage>(s => s.Name("Skip message").Input(step => step.Message, data => "Skip transaction: " + data.CurrentTx))
                                    .Schedule(d => TimeSpan.FromSeconds(10))
                                    .Do(sb => sb.StartWith<CustomMessage>(cfgMessage).Then(checkStatusStep));
                            });

                        checkStatusStep
                            .When(d => ExchangeOperationStatus.Failed, "Status Failed")
                            .Do(doBuilder =>
                            {
                                doBuilder
                                    .StartWith<CustomMessage>(s => s.Name("Fail message").Input(step => step.Message, data => "Failed transaction: " + data.CurrentTx))
                                    .Then<FailedTransaction>(s => s.Name("Mark as failed").Input(st => st.Transaction, d => d))
                                    .Then<Refund>(s => s.Name("Refund").Input(step => step.Transaction, data => data))
                                    .EndWorkflow();
                            });
                    })
                )
                .CompensateWith<CustomMessage>(b => b.Name("Fail message").Input(step => step.Message, data => "Failed transaction: " + data.CurrentTx))
                    .Then<FailedTransaction>(b => b.Name("Mark as failed").Input(st => st.Transaction, d => d))
                    .Then<Refund>(b => b.Name("Refund").Input(step => step.Transaction, data => data))
                .EndWorkflow()
                .OnError(WorkflowErrorHandling.Compensate);
        }
    }
}
