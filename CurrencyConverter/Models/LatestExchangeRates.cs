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

        public string base { get; set; }

        public string date { get; set; }

        public Dictionary<string, double> rates { get; set; }

    }
}
