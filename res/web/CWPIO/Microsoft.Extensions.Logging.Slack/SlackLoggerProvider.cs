using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace Microsoft.Extensions.Logging.Slack
{

    public class SlackLoggerProvider : ILoggerProvider
    {
        private readonly string _applicationName;
        private readonly SlackConfiguration _configuration;
        private readonly string _environmentName;
        private readonly Func<string, LogLevel, Exception, bool> _filter;
        private readonly HttpClient _httpClient;

        public SlackLoggerProvider(IOptions<SlackConfiguration> configuration, HttpClient httpClient,
            string applicationName, string environmentName)
        {
            _configuration = configuration.Value;
            _filter = (n, l, e) => l >= this._configuration.MinLevel;
            _httpClient = httpClient ?? new HttpClient();
            _applicationName = applicationName;
            _environmentName = environmentName;
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Creates a new <see cref="ILogger"/> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new SlackLogger(categoryName, _filter, _httpClient, _environmentName, _applicationName, _configuration.WebhookUrl);
        }
    }
}