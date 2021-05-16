using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.BLL.Dtos
{
    public class CreateConversionDto
    {
        [Required]
        [JsonProperty("fromCurrency")]
        [MinLength(3)]
        [MaxLength(3)]
        public string FromCurrency { get; set; }

        [Required]
        [JsonProperty("toCurrency")]
        [MinLength(3)]
        [MaxLength(3)]
        public string ToCurrency { get; set; }

        [Required]
        [JsonProperty("value")]
        public float Value { get; set; }

    }
}
