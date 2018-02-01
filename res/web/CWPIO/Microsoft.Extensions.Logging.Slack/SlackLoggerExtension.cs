using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace Microsoft.Extensions.Logging.Slack
{
    public static class SlackLoggerExtension
    {
        public static ILoggingBuilder AddSlack(this ILoggingBuilder builder, string applicationName, string environmentName, HttpClient client = null)
        {
            if (string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentNullException(nameof(applicationName));
            }

            if (string.IsNullOrEmpty(environmentName))
            {
                throw new ArgumentNullException(nameof(environmentName));
            }

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, SlackLoggerProvider>(s =>
            {
                var config = s.GetService<IOptions<SlackConfiguration>>();
                return new SlackLoggerProvider(config, client, applicationName, environmentName);
            }));

            return builder;
        }

        public static ILoggingBuilder AddSlack(this ILoggingBuilder builder, Action<SlackConfiguration> configuration, string applicationName, string environmentName, HttpClient client = null)
        {
            builder.Services.Configure(configuration);
            return builder.AddSlack(applicationName, environmentName, client);
        }

        public static ILoggingBuilder AddSlack(this ILoggingBuilder builder, IHostingEnvironment hostingEnvironment, HttpClient client = null)
        {
            return builder.AddSlack(hostingEnvironment.ApplicationName, hostingEnvironment.EnvironmentName, client);
        }

        public static ILoggingBuilder AddSlack(this ILoggingBuilder builder, Action<SlackConfiguration> configuration, IHostingEnvironment hostingEnvironment, HttpClient client = null)
        {
            builder.Services.Configure(configuration);
            return builder.AddSlack(hostingEnvironment.ApplicationName, hostingEnvironment.EnvironmentName, client);
        }
    }
}