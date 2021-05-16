using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CurrencyConverter.BLL.Services;
using CurrencyConverter.BLL.Dtos;

namespace CurrencyConverter.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/currencies")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            this._currencyService = currencyService;
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
            return Ok(this._currencyService.GetAllAvailableCurrencies());
        }

        /// <summary>
        /// Get all exchange rate for currencies by {currencyCode}
        /// </summary>
        /// <param name="currencyCode"></param>
        
        [HttpGet("{currencyCode}")]
        public ActionResult<ShowExchangeRateDto> GetRatesByCurrency(string currencyCode)
        {
            ShowExchangeRateDto showExchangeRateDto = this._currencyService.GetAllConversionRatesByCurrency(currencyCode);

            if (showExchangeRateDto.Base is null)
            {
                return NotFound();
            }

            return Ok(showExchangeRateDto);
        }

        /// <summary>
        /// Get historic exchange rate for currencies by {days}
        /// </summary>
        /// <param name="days"></param>
        
        [HttpGet("historic/{days}")]
        public ActionResult<ShowExchangeRateDto> GetHistoricRateByDate(int days)
        {
            DateTime currentDate = DateTime.Now;
            ShowExchangeRateDto showExchangeRateDto = this._currencyService.GetHistoricRateByDate(currentDate.AddDays(-days));

            if (showExchangeRateDto.Base is null)
            {
                return NotFound();
            }

            return Ok(showExchangeRateDto);
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
        public ActionResult<ShowConversionDto> ConvertSourceToDestinationCurrency([FromBody]CreateConversionDto createConversionDto)
        {
            ShowConversionDto showConversionDto = this._currencyService.Convert(createConversionDto);
            
            if (showConversionDto.FromCurrency is null)
            {
                return NotFound();
            }

            return Ok(showConversionDto);
        }
    }
}
