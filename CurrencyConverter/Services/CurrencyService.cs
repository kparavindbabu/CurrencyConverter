using CurrencyConverter.Interfaces;
using CurrencyConverter.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
            this.accessKey = configuration.GetValue<string>("currencyApiKey");
        }

        public async Task<string> ConvertSourceToDestinationCurrency(CreateConversion data)
        {
            string apiUrl = $"/convert?access_key={this.accessKey}&from={data.fromCurrency}"+ 
                            $"&to={data.toCurrency}&amount={data.value}";
            var response = await _httpClient.GetAsync(apiUrl);
            return await response.Content.ReadAsStringAsync();
        }

        public LatestExchangeRates GetAllConversionRatesByCurrency(string currencyCode)
        {
            LatestExchangeRates latestExchangeRates;

            string apiUrl = $"/latest?access_key={this.accessKey}&base={currencyCode}";

            HttpResponseMessage getResponseMessage = _httpClient.GetAsync(apiUrl).Result;

            if (!getResponseMessage.IsSuccessStatusCode)
                throw new Exception(getResponseMessage.ToString());

            var responseMessage = getResponseMessage.Content.ReadAsStringAsync().Result;

            dynamic project = JsonConvert.DeserializeObject(responseMessage);

            latestExchangeRates = project.ToObject<LatestExchangeRates>();

            return latestExchangeRates;
        }

        public async Task<string> GetHistoricRateByDate(DateTime dateval)
        {
            var historyDate = dateval.ToString("yyyy-MM-dd");
            string apiUrl = $"/{historyDate}?access_key={this.accessKey}&base=EUR";
            var response = await _httpClient.GetAsync(apiUrl);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
