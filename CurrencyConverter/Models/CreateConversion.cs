using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.Models
{
    public class CreateConversion
    {
        [Required]
        [JsonProperty("fromCurrency")]
        public string FromCurrency { get; set; }

        [Required]
        [JsonProperty("toCurrency")]
        public string ToCurrency { get; set; }

        [Required]
        [JsonProperty("value")]
        public float Value { get; set; }

    }
}
