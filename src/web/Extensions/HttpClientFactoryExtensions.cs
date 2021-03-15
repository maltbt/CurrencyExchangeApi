using CurrencyExchangeAPI.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace CurrencyExchangeAPI.Extensions
{
    public interface IHttpClientFactoryExtensions
    {
        public HttpClient Create(string clientId, string baseUri);
    }
    public class HttpClientFactoryExtensions : IHttpClientFactoryExtensions
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly ILogFormatter _logFormatter;

        public HttpClientFactoryExtensions(IHttpClientFactory httpClientFactory,
            HttpClient httpClient,
            ILogger<HttpClientFactoryExtensions> logger,
            ILogFormatter logFormatter
        )
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
            _logger = logger;
            _logFormatter = logFormatter;
        }
        public HttpClient Create(string clientId, string baseUri)
        {
            try
            {
                _httpClient = _httpClientFactory.CreateClient(clientId);
                _httpClient.BaseAddress = new Uri(baseUri);
                return _httpClient;
            }
            catch (Exception e)
            {
                _logger.LogError(_logFormatter.FormatMessage(LogType.Error, ""), e);
                throw;
            }
        }
    }
}
