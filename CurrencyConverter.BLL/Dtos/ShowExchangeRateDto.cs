using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyConverter.BLL.Dtos
{
    public class ShowExchangeRateDto
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, float> Rates { get; set; }
    }
}
