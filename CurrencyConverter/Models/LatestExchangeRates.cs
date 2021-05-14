using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.Models
{
    public class LatestExchangeRates
    {
        public bool success { get; set; }
        public int timestamp { get; set; }
        public string _base { get; set; }
        public string date { get; set; }
        public IEnumerable<Dictionary<string, float>> rates { get; set; }

    }
}
