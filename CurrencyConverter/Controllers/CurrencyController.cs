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
    [Produces("application/json")]
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
        public ActionResult<Array> GetAllCurrencies()
        {
            return Ok(_currencyRepository.GetAllAvailableCurrencies());
        }

        /// <summary>
        /// Get all exchange rate for currencies by {currencyCode}
        /// </summary>
        /// <param name="currencyCode"></param>
        
        [HttpGet("{currencyCode}")]
        public ActionResult<LatestExchangeRates> GetRatesByCurrency(string currencyCode)
        {
            LatestExchangeRates latestExchangeRates = _currencyRepository.GetAllConversionRatesByCurrency(currencyCode);

            return Ok(latestExchangeRates);
        }

        /// <summary>
        /// Get historic exchange rate for currencies by {days}
        /// </summary>
        /// <param name="days"></param>
        
        [HttpGet("historic/{days}")]
        public ActionResult<HistoricExchangeRates> GetHistoricRateByDate(int days)
        {
            DateTime currentDate = DateTime.Now;
            HistoricExchangeRates historicExchangeRates = _currencyRepository.GetHistoricRateByDate(currentDate.AddDays(-days));

            return Ok(historicExchangeRates);
        }

        /// <summary>
        /// Convert currency value from one to other.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/currencies
        ///     {
        ///        "fromCurrency": "EUR",
        ///        "toCurrency": "GBP",
        ///        "value": 10
        ///     }
        ///
        /// </remarks>
        /// <returns>Converted currency value</returns>
        /// <response code="200">Returns converted currency value</response>
        /// <response code="400">If the something wrong</response> 

        [HttpPost]
        public ActionResult<ShowConversion> ConvertSourceToDestinationCurrency([FromBody]CreateConversion data)
        {
            try
            {
                ShowConversion showConversion = _currencyRepository.Convert(data);
                return Ok(showConversion);
            }
            catch(NotSupportedException e)
            {
                return NotFound();
            }
            
        }
    }
}
