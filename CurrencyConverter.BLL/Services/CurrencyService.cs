using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            this._currencyRepository = currencyRepository;
        }

        public ShowConversionDto Convert(CreateConversionDto createConversionDto)
        {
            ShowConversionDto showConversionDto = CalculateValue(createConversionDto);

            return showConversionDto;
        }

        private ShowConversionDto CalculateValue(CreateConversionDto createConversionDto)
        {
            LatestExchangeRates latestExchangeRates = _currencyRepository.GetLatestConversionRatesByCurrency("EUR");
            ShowConversionDto showConversionDto = new ShowConversionDto(createConversionDto.FromCurrency, createConversionDto.ToCurrency, createConversionDto.Value);

            showConversionDto.ExchangeRate = CalculateExchangeRateValue(showConversionDto, latestExchangeRates);
            showConversionDto.Result = showConversionDto.ExchangeRate * createConversionDto.Value;

            return showConversionDto;
        }

        private float CalculateExchangeRateValue(ShowConversionDto showConversionDto, LatestExchangeRates latestExchangeRates)
        {
            float fromVsBaseExchangeRate = showConversionDto.FromCurrency == latestExchangeRates.Base ? 1 : FindExchangeRate(latestExchangeRates, showConversionDto.FromCurrency);
            float baseVsToExchangeRate = showConversionDto.ToCurrency == latestExchangeRates.Base ? 1 : FindExchangeRate(latestExchangeRates, showConversionDto.ToCurrency);

            if (fromVsBaseExchangeRate > 0)
            {
                return 1 / fromVsBaseExchangeRate * baseVsToExchangeRate;
            }

            throw new NotSupportedException($"Conversion from {showConversionDto.FromCurrency} to {showConversionDto.ToCurrency} is not supported at the moment");
        }

        private float FindExchangeRate(LatestExchangeRates latestExchangeRates, string toCurrency)
        {
            if (latestExchangeRates.Rates.TryGetValue(toCurrency, out float exchangeRate))
            {
                return exchangeRate;
            }

            return 0;
        }

        public ShowExchangeRateDto GetAllConversionRatesByCurrency(string currencyCode)
        {
            LatestExchangeRates latestExchangeRates = _currencyRepository.GetLatestConversionRatesByCurrency("EUR");
           
            return ConvertAsBaseCurrency(latestExchangeRates, currencyCode); ;
        }

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

        public Array GetAllAvailableCurrencies()
        {
            ShowExchangeRateDto showExchangeRateDto = GetAllConversionRatesByCurrency("EUR");

            Array currencyList = showExchangeRateDto.Rates.Keys.ToArray();

            return currencyList;
        }

    }
}
