using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CurrencyConverter.DAL
{
    public class CurrencyApi : ICurrencyApi
    {
        private readonly string _accessKey;
        private readonly HttpClient _httpClient;

        public CurrencyApi(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this._httpClient = httpClientFactory.CreateClient("currency");
            this._accessKey = configuration.GetValue<string>("currencyApi:Key");
        }

        public string Call(string apiUrl)
        {
            apiUrl += $"&access_key={this._accessKey}";

            try
            {
                HttpResponseMessage getResponseMessage = this._httpClient.GetAsync(apiUrl).Result;

                if (!getResponseMessage.IsSuccessStatusCode)
                    throw new Exception(getResponseMessage.ToString());

                return getResponseMessage.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
