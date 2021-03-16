# Introduction
The CurrencyExchange API uses the EuropeanBank exchnage rate API to convert an amount to a given target currency.

The API receives an amount, source currency and target currency. It returns the converted amount 
and target currency.
The currency exchange rates is the latest available rates from https://api.exchangeratesapi.io

# Technical Notes
This project uses .Net Core 5.0 and C#.

To run the application

1. Navigate to `.\src\web`

3. Run project `dotnet run`

4. Navigate to Swagger `https://localhost:5001/`

To run the integration tests

1. Navigate to `.\src\CurrencyExchange.IntegrationTests`

3. Run project `dotnet test`

To run the integration tests

1. Navigate to `.\src\CurrencyExchange.UnitTests`

3. Run project `dotnet test`