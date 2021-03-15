using System;

namespace CurrencyExchangeAPI.Services
{
    public class CurrencyStringService
    {
        public static bool IsAllUpper(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!Char.IsUpper(input[i]))
                    return false;
            }

            return true;
        }

    }
}
