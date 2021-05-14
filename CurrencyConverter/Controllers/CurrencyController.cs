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
        public async Task<string> GetAllCurrencies()
        {
            return await _currencyRepository.GetAllConversionRatesByCurrency("");
        }

        [HttpGet("{currencyCode}")]
        public async Task<string> GetRatesByCurrency(string currencyCode)
        {
            return await _currencyRepository.GetAllConversionRatesByCurrency(currencyCode);
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
        public async Task<object> ConvertSourceToDestinationCurrency(CreateConvertion data)
        {
            Console.WriteLine(data);
            return await _currencyRepository.ConvertSourceToDestinationCurrency(data);
            //return await _currencyRepository.ConvertSourceToDestinationCurrency("");
        }
    }
}
