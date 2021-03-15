using CurrencyExchangeAPI;
using CurrencyExchangeAPI.Features.CurrencyExchange;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace CurrencyExchange.IntegrationTests.Features
{
    public class CurrencyExchangeControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly string _uriPath = "/api/CurrencyExchange/Convert";
        private readonly HttpClient _httpClient;
        public CurrencyExchangeControllerTests(WebApplicationFactory<Startup> webappFactory)
        {
            _httpClient = webappFactory.CreateClient();
            //Refactor: get base address from appsettings.test.json
            _httpClient.BaseAddress = new Uri("https://localhost:5001");
        }

        //ToDo: Add more test to check the expected error messages for currency
        // & amount validation.
        // Note: applied Red Green pattern

        [Fact]
        public async void RequestWithBasicValidData_ReturnsOk()
        {
            //Assign
            var requestBody = new CurrencyExchangeModel()
            {
                Amount = 10M,
                SourceCurrency = "GBP",
                TargetCurrency = "USD"
            };
            var request = new HttpRequestMessage(HttpMethod.Get, _uriPath)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8, "application/json")
            };

            //Act
            var response = await _httpClient.SendAsync(request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void RequestWithInvalidSourceCurrency_ReturnsInternalServerError()
        {
            //Assign
            var requestBody = new CurrencyExchangeModel()
            {
                Amount = 10M,
                SourceCurrency = "GBPPPP",
                TargetCurrency = "USD"
            };
            var request = new HttpRequestMessage(HttpMethod.Get, _uriPath)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8, "application/json")
            };

            //Act
            var response = await _httpClient.SendAsync(request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async void RequestWithInvalidTargetCurrency_ReturnsInternalServerError()
        {
            //Assign
            var requestBody = new CurrencyExchangeModel()
            {
                Amount = 10,
                SourceCurrency = "GBP",
                TargetCurrency = ""
            };
            var request = new HttpRequestMessage(HttpMethod.Get, _uriPath)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8, "application/json")
            };

            //Act
            var response = await _httpClient.SendAsync(request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async void RequestWithExtraFieldsInRequest_ReturnsOk()
        {
            // Testing robustness principle
            //Assign
            var requestBody = new
            {
                Amount = 10,
                SourceCurrency = "GBP",
                TargetCurrency = "USD",
                TargetCurrencyZZ = "USD"
            };
            var request = new HttpRequestMessage(HttpMethod.Get, _uriPath)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8, "application/json")
            };

            //Act
            var response = await _httpClient.SendAsync(request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void RequestWithInvalidFieldName_ReturnsInternalServer()
        {
            //Assign
            var requestBody = new
            {
                Amount = 10,
                SourceCurrency = "GBP",
                TargetCurrencyZZ = "USD"
            };
            var request = new HttpRequestMessage(HttpMethod.Get, _uriPath)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8, "application/json")
            };

            //Act
            var response = await _httpClient.SendAsync(request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
