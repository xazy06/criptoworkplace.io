﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
                .AddEnvironmentVariables().Build();

            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // entry to run app
            serviceProvider.GetService<App>().Run(Configuration.GetValue<bool>("RUNNING_IN_CONTAINER")).Wait();
            while(Console.ReadKey().Key != ConsoleKey.Escape)
            { }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // add logging
            serviceCollection.AddSingleton(new LoggerFactory().AddConsole(LogLevel.Debug));
            serviceCollection.AddLogging();

            //add  options
            serviceCollection.AddOptions();
            serviceCollection.Configure<EthSettings>(Configuration.GetSection("Ether"));

            // add services
            serviceCollection.AddSingleton<Eth>();
            serviceCollection.AddSingleton(s =>
                new Database(Configuration.GetConnectionString("CWPConnection"), s.GetRequiredService<ILogger<Database>>())
            );

            // add app
            serviceCollection.AddTransient<App>();
        }
    }
}
