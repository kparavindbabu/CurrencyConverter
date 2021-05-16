using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using CurrencyConverter.BLL.Services;
using CurrencyConverter.BLL.Dtos;
using System.Net.Mime;

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

        #region "GET /api/currencies - List of currencies supported"
        /// <summary>
        ///     Get all currencies
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/currencies
        ///
        /// </remarks>
        /// <returns>Converted currency value</returns>
        /// <response code="200">Returns converted currency value</response>
        /// <response code="400">If the validation fails</response> 
        /// <response code="404">Returned if no data is available</response>  

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ShowExchangeRateDto), StatusCodes.Status200OK)]
        public ActionResult<Array> GetAllCurrencies()
        {
            try
            {
                return Ok(this._currencyService.GetAllAvailableCurrencies());
            }
            catch (Exception)
            {
                // need to log
                return NotFound();
            }
        }
        #endregion

        #region"GET /api/currencies/{currencyCode} - Get Exchange rates based on currency code"
        /// <summary>
        /// Get all exchange rate related to base currency {currencyCode}
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/currencies/EUR
        ///
        /// </remarks>
        /// <param name="currencyCode">Input the company code like EUR, GBP, USD, INR, etc,.</param>
        /// <response code="200">Returns converted currency value</response>
        /// <response code="400">If the validation fails</response> 
        /// <response code="404">Returned if no data is available</response>  

        [HttpGet("{currencyCode}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ShowExchangeRateDto), StatusCodes.Status200OK)]
        public ActionResult<ShowExchangeRateDto> GetRatesByCurrency(string currencyCode)
        {
            try
            {
                ShowExchangeRateDto showExchangeRateDto = this._currencyService.GetAllConversionRatesByCurrency(currencyCode);

                if (showExchangeRateDto.Base is null)
                {
                    // need to log
                    return NotFound();
                }

                return Ok(showExchangeRateDto);
            }
            catch (Exception)
            {
                // need to log
                return NotFound();
            }
        }
        #endregion

        #region"GET /api/currencies/historic/10 - Get exchange rates for that historic day"
        /// <summary>
        /// Get historic exchange rate for currencies by {days}
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/currencies/historic/10
        ///
        /// </remarks>
        /// <param name="days">N</param>
        /// <response code="200">Returns converted currency value</response>
        /// <response code="400">If the validation fails</response> 
        /// <response code="404">Returned if no data is available</response>

        [HttpGet("historic/{days}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ShowExchangeRateDto), StatusCodes.Status200OK)]
        public ActionResult<ShowExchangeRateDto> GetHistoricRateByDate(int days)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                ShowExchangeRateDto showExchangeRateDto = this._currencyService.GetHistoricRateByDate(currentDate.AddDays(-days));

                if (showExchangeRateDto.Base is null)
                {
                    // need to log
                    return NotFound();
                }

                return Ok(showExchangeRateDto);
            }
            catch (Exception)
            {
                // need to log
                return NotFound();
            }
        }
        #endregion

        #region "POST /api/currencies - Convert base value to target currency"
        /// <summary>
        ///     Convert currency value from base currency to another.
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
        /// <response code="400">If client validation fails</response> 
        /// <response code="404">Returned if no data is available</response>

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ShowConversionDto), StatusCodes.Status200OK)]
        public ActionResult<ShowConversionDto> ConvertSourceToDestinationCurrency([FromBody]CreateConversionDto createConversionDto)
        {
            try
            {
                ShowConversionDto showConversionDto = this._currencyService.Convert(createConversionDto);
                if (showConversionDto.FromCurrency is null)
                {
                    // need to log
                    return NotFound();
                }

                return Ok(showConversionDto);
            }
            catch (Exception)
            {
                // need to log
                return NotFound();
            }
        }
        #endregion
    }
}
