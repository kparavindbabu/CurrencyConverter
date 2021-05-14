using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.Models
{
    public class ShowConversion
    {
        public ShowConversion(string fromCurrency, string toCurrency, float baseValue)
        {
            this.FromCurrency = fromCurrency;
            this.ToCurrency = toCurrency;
            this.BaseValue = baseValue;
        }

        [Required]
        [JsonProperty("FromCurrency")]
        public string FromCurrency { get; set; }

        [Required]
        [JsonProperty("ToCurrency")]
        public string ToCurrency { get; set; }

        [Required]
        [JsonProperty("BaseValue")]
        public float BaseValue { get; set; }

        [Required]
        [JsonProperty("ExchangeRate")]
        public float ExchangeRate { get; set; }

        [Required]
        [JsonProperty("Result")]
        public float Result { get; set; }

    }
}
