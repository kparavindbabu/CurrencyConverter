using CurrencyConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyConverter.Interfaces
{
    public interface ICurrencyService
    {
        Array GetAllAvailableCurrencies();

        LatestExchangeRates GetAllConversionRatesByCurrency(string currencyCode);

        ShowConversion Convert(CreateConversion data);

        HistoricExchangeRates GetHistoricRateByDate(DateTime dateval);

    }
}
