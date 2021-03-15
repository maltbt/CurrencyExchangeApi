namespace CurrencyExchangeAPI.Services
{
    public class ConfigurationOptions
    {
        public ConfigurationOptions()
        {
            BaseUri = BaseUri;
            Publisher = Publisher;
        }

        public const string ThirdPartyApi = "ThirdPartyApi";
        public string Publisher { get; set; }
        public string BaseUri { get; set; }
    }
}
