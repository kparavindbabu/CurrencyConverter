﻿using CurrencyConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyConverter.Interfaces
{
    public interface ICurrencyService
    {
        LatestExchangeRates GetAllConversionRatesByCurrency(string currencyCode);

        Task<string> ConvertSourceToDestinationCurrency(CreateConversion data);
        Task<string> GetHistoricRateByDate(DateTime dateval);

    }
}