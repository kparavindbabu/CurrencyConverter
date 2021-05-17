using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        // Summary:
        //   Calls the currency API provider endpoints through HttpClient 
        //
        // Parameters:
        //   string apiUrl:
        //     Actual end point to be called.
        // Return:
        //   string 
        //     Http response as string
        public async Task<string> Call(string apiUrl)
        {
            apiUrl += $"&access_key={this._accessKey}";

            try
            {
                HttpResponseMessage response = await this._httpClient.GetAsync(apiUrl);

                response.EnsureSuccessStatusCode();
                // string responseBody = await response.Content.ReadAsStringAsync();

                //if (!getResponseMessage.IsSuccessStatusCode)
                //    throw new Exception(getResponseMessage.ToString());

                return await response.Content.ReadAsStringAsync(); ;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
