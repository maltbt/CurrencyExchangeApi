using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Features.CurrencyExchange
{
    public class ConvertedCurrencyDto
    {
        public decimal ConvertedAmount { get; set; }
        public string TargetCurrency { get; set; }
    }
}
