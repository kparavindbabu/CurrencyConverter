using CurrencyConverter.Interfaces;
using CurrencyConverter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyConverter.Controllers
{
    [Route("api/currencies")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyRepository;

        public CurrencyController(ICurrencyService currencyRepository)
        {
            this._currencyRepository = currencyRepository;
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/currencies
        ///
        /// </remarks>
        /// <returns>Converted currency value</returns>
        /// <response code="200">Returns converted currency value</response>
        /// <response code="400">If the something wrong</response> 

        [HttpGet]
        public ActionResult<LatestExchangeRates> GetAllCurrencies()
        {
            LatestExchangeRates latestExchangeRates;

            latestExchangeRates = _currencyRepository.GetAllConversionRatesByCurrency("");

            return Ok(latestExchangeRates);
        }

        /// <summary>
        /// Get all exchange rate for currencies by {currencyCode}
        /// </summary>
        /// <param name="currencyCode"></param>
        
        [HttpGet("{currencyCode}")]
        public ActionResult<LatestExchangeRates> GetRatesByCurrency(string currencyCode)
        {
            LatestExchangeRates latestExchangeRates;

            latestExchangeRates = _currencyRepository.GetAllConversionRatesByCurrency(currencyCode);

            return Ok(latestExchangeRates);
        }

        /// <summary>
        /// Get historic exchange rate for currencies by {days}
        /// </summary>
        /// <param name="days"></param>
        
        [Route("historic/{days}")]
        [HttpGet("{days}")]
        public async Task<string> GetHistoricRateByDate(int days)
        {
            DateTime currentDate = DateTime.Now;
            currentDate = currentDate.AddDays(-days);
            return await _currencyRepository.GetHistoricRateByDate(currentDate);
        }

        /// <summary>
        /// Convert currency value from one to other.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/currencies
        ///     {
        ///        "value": 10,
        ///        "fromCurrency": "EUR",
        ///        "toCurrency": "GBP"
        ///     }
        ///
        /// </remarks>
        /// <returns>Converted currency value</returns>
        /// <response code="200">Returns converted currency value</response>
        /// <response code="400">If the something wrong</response> 

        [HttpPost]
        public async Task<object> ConvertSourceToDestinationCurrency(CreateConversion data)
        {
            Console.WriteLine(data);
            return await _currencyRepository.ConvertSourceToDestinationCurrency(data);
            //return await _currencyRepository.ConvertSourceToDestinationCurrency("");
        }
    }
}
