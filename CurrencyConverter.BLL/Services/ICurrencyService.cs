using CurrencyConverter.BLL.Dtos;
using System;
using System.Threading.Tasks;

namespace CurrencyConverter.BLL.Services
{
    public interface ICurrencyService
    {
        Task<ListCurrencyDto> GetAllAvailableCurrencies();

        Task<ShowExchangeRateDto> GetAllConversionRatesByCurrency(string currencyCode);

        Task<ShowConversionDto> Convert(CreateConversionDto createConversionDto);

        Task<ShowExchangeRateDto> GetHistoricRateByDate(DateTime dateval);

    }
}
