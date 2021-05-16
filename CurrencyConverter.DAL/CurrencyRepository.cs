using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using CurrencyConverter.DAL.Models;

namespace CurrencyConverter.DAL
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ICurrencyApi _currencyApi;

        public CurrencyRepository(ICurrencyApi currencyApi)
        {
            this._currencyApi = currencyApi;
        }

        public LatestExchangeRates GetLatestConversionRatesByCurrency(string currencyCode)
        {
            string apiUrl = $"/latest?base=EUR";

            var responseMessage = this._currencyApi.Call(apiUrl);

            LatestExchangeRates latestExchangeRates = JsonConvert.DeserializeObject<LatestExchangeRates>(responseMessage);

            return latestExchangeRates;
        }

        public HistoricExchangeRates GetConversionRatesByDate(DateTime dateval)
        {
            var historyDate = dateval.ToString("yyyy-MM-dd");

            string apiUrl = $"/{historyDate}?base=EUR";

            var responseMessage = this._currencyApi.Call(apiUrl);

            HistoricExchangeRates historicExchangeRates = JsonConvert.DeserializeObject<HistoricExchangeRates>(responseMessage);

            return historicExchangeRates;
        }


    }
}
