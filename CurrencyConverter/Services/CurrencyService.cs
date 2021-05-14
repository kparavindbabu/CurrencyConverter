using CurrencyConverter.Models;
using CurrencyConverter.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyConverter.Services
{
    public class CurrencyService : ICurrencyRepository
    {
        private readonly string accessKey;
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient, IConfiguration configuration)
        {
            this._httpClient = httpClient;
            this.accessKey = configuration.GetValue<string>("currencyApiKey");
        }

        public async Task<string> ConvertSourceToDestinationCurrency(CreateConvertion data)
        {
            string APIURL = $"/convert?access_key={this.accessKey}&from={data.fromCurrency}"+ 
                            $"&to={data.toCurrency}&amount={data.value}";
            var response = await _httpClient.GetAsync(APIURL);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAllConversionRatesByCurrency(string currencyCode)
        {
            string APIURL = $"/latest?access_key={this.accessKey}&base={currencyCode}";
            var response = await _httpClient.GetAsync(APIURL);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetHistoricRateByDate(DateTime dateval)
        {
            var historyDate = dateval.ToString("yyyy-MM-dd");
            string APIURL = $"/{historyDate}?access_key={this.accessKey}&base=EUR";
            var response = await _httpClient.GetAsync(APIURL);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
