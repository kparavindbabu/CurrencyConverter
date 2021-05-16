using CurrencyConverter.BLL.Dtos;
using System;

namespace CurrencyConverter.BLL.Services
{
    public interface ICurrencyService
    {
        Array GetAllAvailableCurrencies();

        ShowExchangeRateDto GetAllConversionRatesByCurrency(string currencyCode);

        ShowConversionDto Convert(CreateConversionDto createConversionDto);

        ShowExchangeRateDto GetHistoricRateByDate(DateTime dateval);

    }
}
