using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.BLL.Dtos
{
    public class ListCurrencyDto
    {
        [JsonProperty("currencies")]
        public string[] Currencies { get; set; }

    }
}
