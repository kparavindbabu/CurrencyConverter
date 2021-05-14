using CurrencyConverter.Models;
using CurrencyConverter.Repository;
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
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyController(ICurrencyRepository currencyRepository)
        {
            this._currencyRepository = currencyRepository;
        }

        [HttpGet]
        public ActionResult<LatestExchangeRates> GetAllCurrencies()
        {
            LatestExchangeRates latestExchangeRates;

            latestExchangeRates = _currencyRepository.GetAllConversionRatesByCurrency("");

            return Ok(latestExchangeRates);
        }

        [HttpGet("{currencyCode}")]
        public ActionResult<LatestExchangeRates> GetRatesByCurrency(string currencyCode)
        {
            LatestExchangeRates latestExchangeRates;

            latestExchangeRates = _currencyRepository.GetAllConversionRatesByCurrency(currencyCode);

            return Ok(latestExchangeRates);
        }

        [Route("historic/{days}")]
        [HttpGet("{days}")]
        public async Task<string> GetHistoricRateByDate(int days)
        {
            DateTime currentDate = DateTime.Now;
            currentDate = currentDate.AddDays(-days);
            return await _currencyRepository.GetHistoricRateByDate(currentDate);
        }

        [HttpPost]
        public async Task<object> ConvertSourceToDestinationCurrency(CreateConversion data)
        {
            Console.WriteLine(data);
            return await _currencyRepository.ConvertSourceToDestinationCurrency(data);
            //return await _currencyRepository.ConvertSourceToDestinationCurrency("");
        }
    }
}
