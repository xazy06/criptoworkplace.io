using ExchangerMonitor.Model;
using ExchangerMonitor.Services;
using ExchangerMonitor.Settings;
using ExchangerMonitor.Workflow;
using ExchangerMonitor.WorkflowSteps;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using WorkflowCore.Interface;

namespace ExchangerMonitor
{
    internal class Program
    {
        public static IConfiguration Configuration { get; private set; }
        private static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables("ASPNETCORE_")
                .AddEnvironmentVariables("DOTNET_")
                .AddEnvironmentVariables()
                .Build();

            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // entry to run app
            bool inContainer = Configuration.GetValue<bool>("RUNNING_IN_CONTAINER");
            var logger = serviceProvider.GetService<ILogger<Program>>();
            var host = serviceProvider.GetRequiredService<IWorkflowHost>();
            host.RegisterWorkflow<MonitorDBWorkflow>();
            host.RegisterWorkflow<BuyTokensWorkflow, ExchangeTransaction>();
            host.RegisterWorkflow<PrintStatusWorkflow, Dictionary<string, ExchangeTransaction>>();
            host.RegisterWorkflow<MonitorWorkflow>();
            host.OnStepError += (w,s,e) => logger?.LogError(e,"Error in workflow {0} at step {1}", w.Reference, string.IsNullOrEmpty(s.Name)? s.Id.ToString(): s.Name);
            host.Start();
            host.StartWorkflow("Monitor DB", reference: "MonitorDB");
            host.StartWorkflow("Monitor", reference: "Monitor");
            if (!inContainer)
            {
                while (Console.ReadKey().Key != ConsoleKey.Escape)
                { }
            }
            else
            {
                while (true)
                { Thread.Sleep(10000); }
            }

            host.Stop();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // add logging
            //serviceCollection.AddSingleton(new LoggerFactory().AddConsole(LogLevel.Information));
            serviceCollection.AddLogging(c =>
            {
                c.AddConfiguration(Configuration.GetSection("Logging"));
                c.AddConsole();
                c.AddDebug();
            });

            //add  options
            serviceCollection.AddOptions();
            serviceCollection.Configure<EthSettings>(Configuration.GetSection("Ether"));

            serviceCollection.AddWorkflow(config =>
            {
                //config.UsePersistence(s => s.);
            });

            //// add services
            serviceCollection.AddSingleton<IEthService, EthService>();
            serviceCollection.AddSingleton<IDatabaseService>(s =>
                new DatabaseService(Configuration.GetConnectionString("CWPConnection"), s.GetRequiredService<ILogger<DatabaseService>>())
            );
            serviceCollection.AddSingleton<ICryptoService,CryptoService>();

            serviceCollection.AddTransient<LoadData>();
            serviceCollection.AddTransient<CheckStatus>();
            serviceCollection.AddTransient<PrintData>();
            serviceCollection.AddTransient<CustomMessage>();
            serviceCollection.AddTransient<SendEth>();
            serviceCollection.AddTransient<Refund>();
            serviceCollection.AddTransient<FailedTransaction>();
            serviceCollection.AddTransient<Finish>();
            serviceCollection.AddTransient<CheckStatus>();
            serviceCollection.AddTransient<SetRate>();
            serviceCollection.AddTransient<LoadMonitorData>();
            serviceCollection.AddTransient<SetGasCount>();

            // add app
            //serviceCollection.AddTransient<App>();
        }
    }
}
