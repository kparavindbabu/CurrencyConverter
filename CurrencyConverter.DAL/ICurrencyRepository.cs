using CurrencyConverter.DAL.Models;
using System;
using System.Threading.Tasks;

namespace CurrencyConverter.DAL
{
    public interface ICurrencyRepository
    {
        Task<HistoricExchangeRates> GetConversionRatesByDate(DateTime dateval);

        Task<LatestExchangeRates> GetLatestConversionRatesByCurrency(string currencyCode);
    }
}