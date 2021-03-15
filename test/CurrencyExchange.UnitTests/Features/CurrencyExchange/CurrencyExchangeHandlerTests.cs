using CurrencyExchangeAPI.Features.CurrencyExchange;
using CurrencyExchangeAPI.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyExchange.UnitTests.Features.CurrencyExchange
{
    public class CurrencyExchangeHandlerTests
    {
        [Fact]
        public async void ConvertUSDToGBP_RetrunsExpectedResult()
        {
            //Assign
            var logger = new NullLogger<CurrencyExchangeHandler>();
            var mockLogFormatter = new Mock<ILogFormatter>();
            var mockExchangeRateService = new Mock<IExchangeRateService>();
            mockExchangeRateService
                .Setup(ex => ex.GetRate(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(1.2M));

            var handler = new CurrencyExchangeHandler(logger, mockLogFormatter.Object, mockExchangeRateService.Object);
            var currencyExchangeRequest = new CurrencyExchangeModel()
            {
                Amount = 9,
                TargetCurrency = "GBP",
                SourceCurrency = "USD"
            };
            var request = new CurrencyExchangeRequest(currencyExchangeRequest);

            //Act
            var actualResponse = await handler.Handle(request, new CancellationToken());
            var convertedAmount = actualResponse.ConvertedAmount;

            //Assert
            convertedAmount.Should().Be(10.8M);
        }
    }
}
