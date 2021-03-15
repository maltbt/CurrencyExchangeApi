using CurrencyExchangeAPI.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Features.CurrencyExchange
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyExchangeController : Controller
    {
        private readonly ILogger _logger;
        private readonly ILogFormatter _logFormatter;
        private readonly IMediator _mediator;
        public CurrencyExchangeController(ILogger<CurrencyExchangeController> logger,
            ILogFormatter logFormatter, IMediator mediator)
        {
            _logger = logger;
            _logFormatter = logFormatter;
            _mediator = mediator;
        }

        /// <summary>
        ///     Dev endpoint: https://localhost:5001/api​/CurrencyExchange​/Convert
        /// </summary>
        /// <param name="currencyExchangeRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Convert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConvertCurrency([FromBody] CurrencyExchangeModel currencyExchangeRequest)
        {
            var trackingId = Guid.NewGuid().ToString();

            //ToDo Refactor: use fluentvalidation validate request and fail fast if an issue
            // & write tests for these cases
            if (currencyExchangeRequest == null)
            {
                _logger.LogError(_logFormatter.FormatMessage(LogType.Error, $"Request body is null {trackingId}"));
                return Problem("Request Body is null or empty  ", statusCode: (int)HttpStatusCode.BadRequest);
            }
            
            if (currencyExchangeRequest.Amount <= 0)
            {
                _logger.LogError(_logFormatter.FormatMessage(LogType.Error, $"Amount should be greater than zero {trackingId}"));
                return Problem("Amount should be greater than zero", statusCode: (int)HttpStatusCode.BadRequest);
            }

            if (currencyExchangeRequest.TargetCurrency.Length != 3 && currencyExchangeRequest.SourceCurrency.Length != 3)
            {
                _logger.LogError(_logFormatter.FormatMessage(LogType.Error, $"Source and target currency should be 3 characters {trackingId}"));
                return Problem("Source and target currency should be 3 characters", statusCode: (int)HttpStatusCode.BadRequest);
            }

            if (currencyExchangeRequest.TargetCurrency.Length != 3 && currencyExchangeRequest.SourceCurrency.Length != 3)
            {
                _logger.LogError(_logFormatter.FormatMessage(LogType.Error, $"Source and target currency should be 3 characters {trackingId}"));
                return Problem("Source and target currency should be 3 characters", statusCode: (int)HttpStatusCode.BadRequest);
            }

            if (!CurrencyStringService.IsAllUpper(currencyExchangeRequest.TargetCurrency) 
                && CurrencyStringService.IsAllUpper(currencyExchangeRequest.SourceCurrency))
            {
                _logger.LogError(_logFormatter.FormatMessage(LogType.Error, $"Source and target currency should be upper case {trackingId}"));
                return Problem("Source and target currency should be upper case", statusCode: (int)HttpStatusCode.BadRequest);
            }

            try
            {
                var result = await _mediator
                    .Send(new CurrencyExchangeRequest(currencyExchangeRequest));
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(_logFormatter.FormatMessage(LogType.Error, "API request failed with Exception"), e);
                Console.WriteLine(_logFormatter.FormatMessage(LogType.Error, "API request failed with Exception"), e);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("Health")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult HealthCheck()
        {
            return Ok();
        }
    }
}
