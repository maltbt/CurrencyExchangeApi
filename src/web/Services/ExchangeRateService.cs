using CurrencyExchangeAPI.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Services
{
    public interface IExchangeRateService
    {
        public Task<decimal> GetRate(string sourceCurrency, string targetCurrency);
    }
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly ILogger _logger;
        private readonly ILogFormatter _logFormatter;
        private readonly IHttpClientFactoryExtensions _clientFactory;
        private readonly string _exchangeRateBaseUri;
        private readonly string _exchangeRatePublisher;

        public ExchangeRateService(ILogger<ExchangeRateService> logger, ILogFormatter logFormatter,
            IHttpClientFactoryExtensions clientFactory,
            IOptionsMonitor<ConfigurationOptions> optionsMonitor)
        {
            _logger = logger;
            _logFormatter = logFormatter;
            _clientFactory = clientFactory;
            _exchangeRateBaseUri = optionsMonitor.CurrentValue.BaseUri;
            if (string.IsNullOrEmpty(_exchangeRateBaseUri))
            {
                _logger.LogError(_logFormatter.FormatMessage(LogType.Error, "Third part API URL is null or empty"));
                throw new ArgumentNullException($"Cannot get the rate using base uri: {_exchangeRateBaseUri} ");
            }

            _exchangeRatePublisher = optionsMonitor.CurrentValue.Publisher;
            if (string.IsNullOrEmpty(_exchangeRatePublisher))
            {
                _logger.LogError(_logFormatter.FormatMessage(LogType.Error, "Third part API publisher is null or empty"));
                throw new ArgumentNullException($"Cannot get the rate using publisher: {_exchangeRatePublisher}");
            }
        }
        public async Task<decimal> GetRate(string sourceCurrency, string targetCurrency)
        {
            try
            {
                using var exchangeRateResponse = await JsonDocument
                    .ParseAsync(await _clientFactory
                        .Create(_exchangeRatePublisher, _exchangeRateBaseUri)
                        .GetStreamAsync(string.Concat("?base=", sourceCurrency)));

                var rateJsonProperty = exchangeRateResponse.RootElement.GetProperty("rates")
                    .EnumerateObject()
                    .FirstOrDefault(rateJp =>
                        string.Equals(rateJp.Name.ToString(), targetCurrency, StringComparison.OrdinalIgnoreCase));

                rateJsonProperty.Value.TryGetDecimal(out var resultVal);

                return resultVal;
            }
            catch (Exception e)
            {
                _logger.LogError(_logFormatter.FormatMessage(LogType.Error, ""), e);

                throw;
            }
        }
    }
}
