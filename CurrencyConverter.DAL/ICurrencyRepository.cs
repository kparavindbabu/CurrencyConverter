using CurrencyConverter.DAL.Models;
using System;

namespace CurrencyConverter.DAL
{
    public interface ICurrencyRepository
    {
        HistoricExchangeRates GetConversionRatesByDate(DateTime dateval);

        LatestExchangeRates GetLatestConversionRatesByCurrency(string currencyCode);
    }
}