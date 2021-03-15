namespace CurrencyExchangeAPI.Features.CurrencyExchange
{
    public class CurrencyExchangeModel
    {
        public decimal Amount { get; set; }
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
    }
}
