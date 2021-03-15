using MediatR;

namespace CurrencyExchangeAPI.Features.CurrencyExchange
{
    public class CurrencyExchangeRequest : IRequest<ConvertedCurrencyDto>
    {
        public CurrencyExchangeRequest(CurrencyExchangeModel currencyExchangeRequest)
        {
            // Refactor option, use a automapper to map the model to this class
            // one class depends on another 
            Amount = currencyExchangeRequest.Amount;
            SourceCurrency = currencyExchangeRequest.SourceCurrency;
            TargetCurrency = currencyExchangeRequest.TargetCurrency;
        }
        public decimal Amount { get; set; }
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
    }
}
