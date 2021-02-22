using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Kubera.General;
using Kubera.General.Services;
using Kubera.General.Extensions;
using Kubera.General.Models;
using Kubera.Business.Models.AlphaVantage;
using System.Linq;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;

namespace Kubera.Business.Services
{
    public class AlphaVantageService : IForexService, IStockService
    {
        private readonly HttpClient _httpClient;
        private readonly IAppSettings _appSettings;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AlphaVantageService> _logger;
        private const string FunctionExchange = "CURRENCY_EXCHANGE_RATE";
        private const string FunctionStock = "TIME_SERIES_INTRADAY";
        private const string FunctionOverviewCompany = "OVERVIEW";

        public AlphaVantageService(HttpClient httpClient,
            IAppSettings appSettings,
            ICacheService cacheService,
            ILogger<AlphaVantageService> logger)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;
            _cacheService = cacheService;
            _logger = logger;
        }

        async ValueTask<IResult<IForexServiceResponse>> IForexService.GetPriceOf(string fromCode, string toCode, CancellationToken cancellationToken = default)
        {
            var code = CalculateKey("forex", fromCode, toCode);
            var existingValue = _cacheService.Get<IForexServiceResponse>(code);

            if (existingValue != null)
                return Result.Success(existingValue);

            var result = await GetPriceOfForexImpl(fromCode, toCode, cancellationToken)
                .ConfigureAwait(false);

            if (result.IsFailure)
            {
                _logger.LogError($"GET {nameof(IForexService)}.{nameof(IForexService.GetPriceOf)}: {result.Error}");
                return result;
            }

            _cacheService.Add(result.Value, code);

            return result;
        }

        async ValueTask<IResult<IStockServiceResponse>> IStockService.GetPriceOf(string stock, string toCode, CancellationToken cancellationToken = default)
        {
            var code = CalculateKey("stock", stock, toCode);
            var existingValue = _cacheService.Get<IStockServiceResponse>(code);

            if (existingValue != null)
                return Result.Success(existingValue);

            var result = await GetPriceOfStockImpl(stock, toCode, cancellationToken)
                .ConfigureAwait(false);

            if (result.IsFailure)
            {
                _logger.LogError($"GET {nameof(IStockService)}.{nameof(IStockService.GetPriceOf)}: {result.Error}");
                return result;
            }

            _cacheService.Add(result.Value, code);

            return result;
        }

        async ValueTask<IResult<IStockCompanyResponse>> IStockService.GetCompany(string company, CancellationToken cancellationToken = default)
        {
            var code = CalculateKey("company", company);
            var existingValue = _cacheService.Get<IStockCompanyResponse>(code);

            if (existingValue != null)
                return Result.Success(existingValue);

            var result = await GetCompanyImpl(company, cancellationToken)
                .ConfigureAwait(false);

            if (result.IsFailure)
            {
                _logger.LogError($"GET {nameof(IStockService)}.{nameof(IStockService.GetCompany)}: {result.Error}");
                return result;
            }

            _cacheService.Add(result.Value, code);

            return result;
        }

        protected virtual string CalculateKey(params object[] keys) => string.Join(".", keys);

        private async ValueTask<IResult<IForexServiceResponse>> GetPriceOfForexImpl(string fromCode, string toCode, CancellationToken cancellationToken = default)
        {
            var qs = new
            {
                function = FunctionExchange,
                from_currency = fromCode,
                to_currency = toCode,
                apikey = _appSettings.AlphaVantageApiKey
            };
            var result = await GetResult<AlphaVantageGetCurrencyExchangeRateResponse>(qs, cancellationToken)
                .ConfigureAwait(false);

            if (result.IsFailure)
                return Result.Failure<IForexServiceResponse>(result.Error);

            if (!decimal.TryParse(result.Value.Result?.Rate,
                                    NumberStyles.Any,
                                    CultureInfo.InvariantCulture,
                                    out var rate))
                return Result.Failure<IForexServiceResponse>($"Invalid rate received {result.Value.Result?.Rate}");

            var response = new AlphaVantageForexServiceResponse
            {
                From = fromCode,
                To = toCode,
                Timestamp = DateTime.TryParse(result.Value.Result.LastRefreshed,
                                              CultureInfo.InvariantCulture,
                                              DateTimeStyles.AssumeUniversal,
                                              out var dt)
                                ? dt
                                : DateTime.UtcNow,
                Rate = rate
            };

            return Result.Success(response);
        }

        private async ValueTask<IResult<IStockServiceResponse>> GetPriceOfStockImpl(string stock, string toCode, CancellationToken cancellationToken = default)
        {
            var qs = new
            {
                function = FunctionStock,
                symbol = stock,
                interval = "5min",
                apikey = _appSettings.AlphaVantageApiKey
            };
            var result = await GetResult<AlphaVantageGetStockRateResponse>(qs, cancellationToken)
                .ConfigureAwait(false);
            var openRate = result.Value.Series.First().Value.Open;

            if (!decimal.TryParse(openRate,
                                    NumberStyles.Any,
                                    CultureInfo.InvariantCulture,
                                    out var rate))
                return Result.Failure<IStockServiceResponse>($"Invalid rate received {openRate}");

            var company = await ((IStockService)this).GetCompany(stock, cancellationToken)
                .ConfigureAwait(false);

            if (company.Value.Currency.Equals(toCode, StringComparison.InvariantCultureIgnoreCase))
            {
                var forexResult = await ((IForexService)this).GetPriceOf(company.Value.Currency, toCode, cancellationToken)
                    .ConfigureAwait(false);

                rate *= forexResult.Value.Rate;
            }

            var response = new AlphaVantageStockServiceResponse
            {
                Stock = stock,
                To = toCode,
                Timestamp = DateTime.TryParse(result.Value.MetaData.LastRefreshed,
                                              CultureInfo.InvariantCulture,
                                              DateTimeStyles.AssumeUniversal,
                                              out var dt)
                                ? dt
                                : DateTime.UtcNow,
                Rate = rate
            };

            return Result.Success(response);
        }

        private async ValueTask<IResult<IStockCompanyResponse>> GetCompanyImpl(string company, CancellationToken cancellationToken = default)
        {
            var qs = new
            {
                function = FunctionOverviewCompany,
                symbol = company,
                apikey = _appSettings.AlphaVantageApiKey
            };
            return await GetResult<AlphaVantageStockCompanyResponse>(qs, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<IResult<T>> GetResult<T>(object queryString, CancellationToken token = default)
        {
            var ub = new UriBuilder("https", "www.alphavantage.co")
            {
                Path = "query",
                Query = queryString.AsQueryString()
            };

            var response = await _httpClient.GetAsync(ub.Uri, token)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                Result.Failure<T>($"Fail to get response for forex/stock service {(int)response.StatusCode}");

            await using var content = await response.Content.ReadAsStreamAsync(token)
                .ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<T>(content, cancellationToken: token)
                .ConfigureAwait(false);

            return Result.Success(result);
        }
    }
}