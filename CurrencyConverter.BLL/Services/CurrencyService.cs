using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyConverter.DAL;
using CurrencyConverter.DAL.Models;
using CurrencyConverter.BLL.Dtos;

namespace CurrencyConverter.BLL.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        
        // Summary:
        //   Constructor initialized with dependency ICurrencyRepository
        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            this._currencyRepository = currencyRepository;
        }

        #region "Convert source to target currency value"
        // Summary:
        //   It will convert the given value from source currency to target 
        //
        // Parameters:
        //   createConversionDto:
        //     An instance to CreateConversionDto.
        // Return:
        //   showConversionDto
        //     An instance to ShowConversionDto
        public ShowConversionDto Convert(CreateConversionDto createConversionDto)
        {
            ShowConversionDto showConversionDto = CalculateValue(createConversionDto);

            return showConversionDto;
        }

        // Summary:
        //   Get the current exchange rate from the API repository and converts as ShowConversionDto.
        //
        // Parameters:
        //   createConversionDto:
        //     An instance to CreateConversionDto.
        // Return:
        //   showConversionDto
        //     An instance to ShowConversionDto
        private ShowConversionDto CalculateValue(CreateConversionDto createConversionDto)
        {
            LatestExchangeRates latestExchangeRates = _currencyRepository.GetLatestConversionRatesByCurrency("EUR");
            
            ShowConversionDto showConversionDto = new ShowConversionDto(createConversionDto.FromCurrency, createConversionDto.ToCurrency, createConversionDto.Value);
            showConversionDto.ExchangeRate = CalculateExchangeRateValue(showConversionDto, latestExchangeRates);

            return showConversionDto;
        }

        // Summary:
        //   Manually converting and mapping the exchange rates from default base currency vaalue to the target currency.
        //
        // Parameters:
        //   LatestExchangeRates:
        //     An instance to LatestExchangeRates.
        //   ShowConversionDto:
        //     An instance to ShowConversionDto.
        // Return:
        //   float
        //     Converted final exchange rate
        private float CalculateExchangeRateValue(ShowConversionDto showConversionDto, LatestExchangeRates latestExchangeRates)
        {
            float fromVsBaseExchangeRate = showConversionDto.FromCurrency == latestExchangeRates.Base ? 1 : FindExchangeRate(latestExchangeRates, showConversionDto.FromCurrency);
            float baseVsToExchangeRate = showConversionDto.ToCurrency == latestExchangeRates.Base ? 1 : FindExchangeRate(latestExchangeRates, showConversionDto.ToCurrency);

            if (fromVsBaseExchangeRate > 0 && baseVsToExchangeRate > 0)
            {
                float finalExchangeRate = (1 / fromVsBaseExchangeRate) * baseVsToExchangeRate;
                return finalExchangeRate;
            }

            throw new NotSupportedException($"Conversion from {showConversionDto.FromCurrency} to {showConversionDto.ToCurrency} is not supported at the moment");
        }

        // Summary:
        //   Manually converting and mapping the exchange rates from default base currency vaalue to the target currency.
        //
        // Parameters:
        //   createConversionDto:
        //     An instance to CreateConversionDto.
        // Return:
        //   showConversionDto
        //     An instance to ShowConversionDto
        private float FindExchangeRate(LatestExchangeRates latestExchangeRates, string toCurrency)
        {
            if (latestExchangeRates.Rates.TryGetValue(toCurrency, out float exchangeRate))
            {
                return exchangeRate;
            }

            return 0;
        }

        #endregion

        #region "Get latest exchange rates for a specific currency"
        // Summary:
        //   Get latest exchange rates for a specific currency. By default it's EUR.
        //
        // Parameters:
        //   string:
        //     Currency code.
        // Return:
        //   showExchangeRateDto
        //     An instance to ShowExchangeRateDto
        public ShowExchangeRateDto GetAllConversionRatesByCurrency(string currencyCode)
        {
            LatestExchangeRates latestExchangeRates = _currencyRepository.GetLatestConversionRatesByCurrency("EUR");
           
            return ConvertAsBaseCurrency(latestExchangeRates, currencyCode); ;
        }

        // Summary:
        //   It internally converts the base currency to the targer currency since API only supports EUR
        //
        // Parameters:
        //   latestExchangeRates:
        //     An instance to LatestExchangeRates
        //   string:
        //     Currency code.
        // Return:
        //   showExchangeRateDto
        //     An instance to ShowExchangeRateDto
        private ShowExchangeRateDto ConvertAsBaseCurrency(LatestExchangeRates latestExchangeRates, string currencyCode)
        {
            ShowExchangeRateDto convertedRates = new ShowExchangeRateDto()
            {
                Base = currencyCode,
                Timestamp = latestExchangeRates.Timestamp,
                Date = latestExchangeRates.Date,
                Rates = new Dictionary<string, float>()
            };

            if (latestExchangeRates.Base == currencyCode)
            {
                convertedRates.Rates = latestExchangeRates.Rates;
                return convertedRates;
            }

            foreach (KeyValuePair<string, float> entry in latestExchangeRates.Rates.ToList())
            {
                ShowConversionDto showConversionDto = new ShowConversionDto(currencyCode, entry.Key, 0);
                convertedRates.Rates.Add(entry.Key, CalculateExchangeRateValue(showConversionDto, latestExchangeRates));
            }

            return convertedRates;
        }

        #endregion

        #region "Get historic exchange rates for a specific day"
        // Summary:
        //   Get conversion rates for historic days.
        //
        // Parameters:
        //   DateTime:
        //     History datetime
        // Return:
        //   ShowExchangeRateDto
        //     An instance to ShowExchangeRateDto
        public ShowExchangeRateDto GetHistoricRateByDate(DateTime dateval)
        {
            HistoricExchangeRates historicExchangeRates = _currencyRepository.GetConversionRatesByDate(dateval);

            ShowExchangeRateDto showExchangeRateDto = new ShowExchangeRateDto()
            {
                Base = historicExchangeRates.Base,
                Timestamp = historicExchangeRates.Timestamp,
                Date = historicExchangeRates.Date,
                Rates = historicExchangeRates.Rates
            };

            return showExchangeRateDto;
        }
        #endregion

        #region "List all currencies supported by the data repository"
        // Summary:
        //   Get the list of currencies supported by the data repository.
        //
        // Return:
        //   ShowExchangeRateDto
        //     An instance to ShowExchangeRateDto
        public ListCurrencyDto GetAllAvailableCurrencies()
        {
            ShowExchangeRateDto showExchangeRateDto = GetAllConversionRatesByCurrency("EUR");

            Array currencyList = showExchangeRateDto.Rates.Keys.ToArray();

            return new ListCurrencyDto{ Currencies = (string[])currencyList };
        }
        #endregion

    }
}
