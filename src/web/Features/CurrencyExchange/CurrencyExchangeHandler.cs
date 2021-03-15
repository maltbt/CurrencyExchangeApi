using CurrencyExchangeAPI.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Features.CurrencyExchange
{
    public class CurrencyExchangeHandler : IRequestHandler<CurrencyExchangeRequest, ConvertedCurrencyDto>
    {
        private readonly ILogger _logger;
        private readonly ILogFormatter _logFormatter;
        private readonly IExchangeRateService _exchangeRateService;
        public CurrencyExchangeHandler(ILogger<CurrencyExchangeHandler> logger, ILogFormatter logFormatter,
            IExchangeRateService exchangeRateService)
        {
            _logger = logger;
            _logFormatter = logFormatter;
            _exchangeRateService = exchangeRateService;
        }

        public async Task<ConvertedCurrencyDto> Handle(CurrencyExchangeRequest request,
            CancellationToken cancellationToken)
        {
            // Note: Applied robustness principle in this case.
            // Another option is to only accepts requests with exact API contract

            try
            {
                var exchangeRate = await _exchangeRateService.GetRate(request.SourceCurrency,
                    request.TargetCurrency);

                var result = new ConvertedCurrencyDto
                {
                    ConvertedAmount = request.Amount * exchangeRate,
                    TargetCurrency = request.TargetCurrency
                };

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(_logFormatter.FormatMessage(LogType.Error, ""), e);
                throw;
            }
        }
    }
}
