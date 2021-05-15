using CurrencyConverter.Interfaces;
using CurrencyConverter.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyConverter.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly string accessKey;
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient, IConfiguration configuration)
        {
            this._httpClient = httpClient;
            this.accessKey = configuration.GetValue<string>("currencyApi:Key");
        }

        public ShowConversion Convert( CreateConversion createConversion)
        {
            ShowConversion showConversion = CalculateValue(createConversion);

            return showConversion;
        }

        private ShowConversion CalculateValue(CreateConversion createConversion)
        {
            LatestExchangeRates latestExchangeRates = GetAllConversionRatesByCurrency("EUR");
            ShowConversion showConversion = new ShowConversion(createConversion.FromCurrency, createConversion.ToCurrency, createConversion.Value);

            showConversion.ExchangeRate = CalculateExchangeRateValue(showConversion, latestExchangeRates);
            showConversion.Result = showConversion.ExchangeRate * createConversion.Value;
 
            return showConversion;
        }

        private float CalculateExchangeRateValue(ShowConversion showConversion, LatestExchangeRates latestExchangeRates)
        {
            float fromVsBaseExchangeRate = (showConversion.FromCurrency == latestExchangeRates.Base )? 1 : FindExchangeRate(latestExchangeRates, showConversion.FromCurrency);
            float baseVsToExchangeRate = (showConversion.ToCurrency == latestExchangeRates.Base) ? 1 : FindExchangeRate(latestExchangeRates, showConversion.ToCurrency);

            if (fromVsBaseExchangeRate > 0)
            {
                return (1 / fromVsBaseExchangeRate) * baseVsToExchangeRate;
            }
            
            throw new NotSupportedException($"Conversion from {showConversion.FromCurrency} to {showConversion.ToCurrency} is not supported at the moment");
        }

        private float FindExchangeRate(LatestExchangeRates latestExchangeRates, string toCurrency)
        {
            if (latestExchangeRates.Rates.TryGetValue(toCurrency, out float exchangeRate))
            {
                return exchangeRate;
            }

            return 0;
        }

        public LatestExchangeRates GetAllConversionRatesByCurrency(string currencyCode)
        {
            string apiUrl = $"/latest?access_key={this.accessKey}&base=EUR";

            var responseMessage = HttpCall(apiUrl);

            LatestExchangeRates latestExchangeRates = JsonConvert.DeserializeObject<LatestExchangeRates>(responseMessage);

            latestExchangeRates = ConvertAsBaseCurrency(latestExchangeRates, currencyCode);

            return latestExchangeRates;
        }

        private LatestExchangeRates ConvertAsBaseCurrency(LatestExchangeRates latestExchangeRates, string currencyCode)
        {
            if(latestExchangeRates.Base == currencyCode)
            {
                return latestExchangeRates;
            }

            LatestExchangeRates convertedRates = new LatestExchangeRates();

            convertedRates.Base = currencyCode;
            convertedRates.Timestamp = latestExchangeRates.Timestamp;
            convertedRates.Date = latestExchangeRates.Date;
            convertedRates.Success = latestExchangeRates.Success;
            convertedRates.Rates = new Dictionary<string, float>();
           
            foreach (KeyValuePair<string, float> entry in latestExchangeRates.Rates.ToList())
            {
                ShowConversion showConversion = new ShowConversion(currencyCode, entry.Key, 0);
                convertedRates.Rates.Add(entry.Key, CalculateExchangeRateValue(showConversion, latestExchangeRates));
            }

            return convertedRates;
        }

        public HistoricExchangeRates GetHistoricRateByDate(DateTime dateval)
        {
            var historyDate = dateval.ToString("yyyy-MM-dd");

            string apiUrl = $"/{historyDate}?access_key={this.accessKey}&base=EUR";

            var responseMessage = HttpCall(apiUrl);

            HistoricExchangeRates historicExchangeRates = JsonConvert.DeserializeObject<HistoricExchangeRates>(responseMessage);

            return historicExchangeRates;
        }
        private string HttpCall(string apiUrl)
        {
            HttpResponseMessage getResponseMessage = _httpClient.GetAsync(apiUrl).Result;

            if (!getResponseMessage.IsSuccessStatusCode)
                throw new Exception(getResponseMessage.ToString());

            return getResponseMessage.Content.ReadAsStringAsync().Result;
        }

        public Array GetAllAvailableCurrencies()
        {
            LatestExchangeRates latestExchangeRates = GetAllConversionRatesByCurrency("EUR");
 
            Array currencyList = latestExchangeRates.Rates.Keys.ToArray();

            return currencyList;
        }

    }
}
