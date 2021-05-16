using Newtonsoft.Json;
using System;
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

        // Summary:
        //   Gets latest currency exchange rates for base currency {currencyCode} 
        //
        // Parameters:
        //   string currencyCode:
        //     Currency codes like EUR, GBP, INR, etc,.
        // Return:
        //   LatestExchangeRates 
        //     Typed response from the Http as LatestExchangeRates
        public LatestExchangeRates GetLatestConversionRatesByCurrency(string currencyCode)
        {
            string apiUrl = $"/latest?base=EUR";

            var responseMessage = this._currencyApi.Call(apiUrl);

            LatestExchangeRates latestExchangeRates = JsonConvert.DeserializeObject<LatestExchangeRates>(responseMessage);

            return latestExchangeRates;
        }

        // Summary:
        //   Gets historic currency exchange rates to the base currency EUR for the date {dateval} 
        //
        // Parameters:
        //   DateTime dateval:
        //     Any Historic DateTime value
        // Return:
        //   HistoricExchangeRates 
        //     Typed response from the Http as HistoricExchangeRates
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
