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
        public string fromCurrency { get; set; }

        [Required]
        public string toCurrency { get; set; }

        [Required]
        public float value { get; set; }

    }
}
