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
                .Then<CheckStatus>(s =>
                {
                    s.Name("Check status")
                        .Input(st => st.Status, d => d.Status)
                        .Input(st => st.Tx, d => d.CurrentTx);

                    s.When(d => ExchangeOperationStatus.Ok, "Status OK").Do(b =>
                    {
                        b
                            .StartWith<CustomMessage>(c =>
                                c
                                    .Name("Ok Message")
                                    .Input(step => step.Message, data => "OK transaction: " + data.CurrentTx))
                            .If(d => (ChangeSteps)d.CurrentStep == ChangeSteps.SetRate).Do(sb =>
                            {
                                sb.StartWith<SetRate>(c => c
                                    .Name("Set Rate")
                                    .Input(step => step.Transaction, data => data))
                                .Schedule(d => TimeSpan.FromSeconds(10))
                                .Do(isb => isb.StartWith<CustomMessage>(cfgMessage).Then(s));
                            })
                            .If(d => (ChangeSteps)d.CurrentStep == ChangeSteps.SendEth).Do(sb =>
                            {
                                sb.StartWith<SendEth>(c => c
                                    .Name("Send Eth")
                                    .Input(step => step.Transaction, data => data))
                                .Schedule(d => TimeSpan.FromSeconds(10))
                                .Do(isb => isb.StartWith<CustomMessage>(cfgMessage).Then(s));
                            })
                            .If(d => (ChangeSteps)d.CurrentStep == ChangeSteps.Refund).Do(sb =>
                            {
                                sb.StartWith<Refund>(c => c
                                    .Name("Refund")
                                    .Input(step => step.Transaction, data => data))
                                .Schedule(d => TimeSpan.FromSeconds(10))
                                .Do(isb => isb.StartWith<CustomMessage>(cfgMessage).Then(s));
                            })
                            .If(d => (ChangeSteps)d.CurrentStep == ChangeSteps.Finish).Do(sb =>
                            {
                                sb.StartWith<Finish>(c => c
                                    .Name("Finish")
                                    .Input(step => step.Transaction, data => data))
                                .EndWorkflow();
                            });
                    });

                    s.When(d => ExchangeOperationStatus.Skip, "Status Skip").Do(b =>
                    {
                        b
                            .StartWith<CustomMessage>(c =>
                                c
                                    .Name("Skip message")
                                    .Input(step => step.Message, data => "Skip transaction: " + data.CurrentTx))
                            .Schedule(d => TimeSpan.FromSeconds(10))
                            .Do(sb =>
                                sb
                                    .StartWith<CustomMessage>(cfgMessage)
                                    .Then(s));
                    });

                    s.When(d => ExchangeOperationStatus.Failed, "Status Failed").Do(b =>
                    {
                        b
                            .StartWith<CustomMessage>(c =>
                                c
                                    .Name("Fail message")
                                    .Input(step => step.Message, data => "Failed transaction: " + data.CurrentTx))
                            //.Then<Refund>()
                            .Then<FailedTransaction>(c =>
                                c
                                    .Name("Mark as failed")
                                    .Input(st => st.Transaction, d => d))
                            .EndWorkflow();
                    });
                })
                .OnError(WorkflowErrorHandling.Retry, TimeSpan.FromSeconds(5));
        }
    }
}
