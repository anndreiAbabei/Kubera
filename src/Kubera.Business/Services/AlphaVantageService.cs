using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Kubera.Business.Exceptions;
using Kubera.General;
using Kubera.General.Services;
using Kubera.General.Extensions;
using Kubera.General.Models;
using Kubera.Business.Models.AlphaVantage;
using System.Linq;

namespace Kubera.Business.Services
{
    public class AlphaVantageService : IForexService, IStockService
    {
        private readonly HttpClient _httpClient;
        private readonly IAppSettings _appSettings;
        private readonly ICacheService _cacheService;
        private const string FunctionExchange = "CURRENCY_EXCHANGE_RATE";
        private const string FunctionStock = "TIME_SERIES_INTRADAY"; 
        private const string FunctionOverviewCompany = "OVERVIEW"; 

        public AlphaVantageService(HttpClient httpClient,
            IAppSettings appSettings,
            ICacheService cacheService)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;
            _cacheService = cacheService;
        }

        async ValueTask<IForexServiceResponse> IForexService.GetPriceOf(string fromCode, string toCode, CancellationToken cancellationToken = default)
        {
            var code = CalculateKey("forex", fromCode, toCode);

            return await _cacheService.GetOrAdd(code, () => GetPriceOfForexImpl(fromCode, toCode, cancellationToken))
                .ConfigureAwait(false);
        }

        async ValueTask<IStockServiceResponse> IStockService.GetPriceOf(string stock, string toCode, CancellationToken cancellationToken = default)
        {
            var code = CalculateKey("stock", stock, toCode);

            return await _cacheService.GetOrAdd(code, () => GetPriceOfStockImpl(stock, toCode, cancellationToken))
                .ConfigureAwait(false);
        }

        async ValueTask<IStockCompanyResponse> IStockService.GetCompany(string company, CancellationToken cancellationToken = default)
        {
            var code = CalculateKey("company", company);

            return await _cacheService.GetOrAdd(code, () => GetCompanyImpl(company, cancellationToken))
                .ConfigureAwait(false);
        }

        protected virtual object[] CalculateKey(params string[] type) => type;

        private async ValueTask<IForexServiceResponse> GetPriceOfForexImpl(string fromCode, string toCode, CancellationToken cancellationToken = default)
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

            if(!decimal.TryParse(result.Result.Rate,
                                    NumberStyles.Any,
                                    CultureInfo.InvariantCulture,
                                    out var rate))
                throw new AlphaVantageException($"Invalid rate received {result.Result.Rate}");

            return new AlphaVantageForexServiceResponse
            {
                From = fromCode,
                To = toCode,
                Timestamp = DateTime.TryParse(result.Result.LastRefreshed,
                                              CultureInfo.InvariantCulture,
                                              DateTimeStyles.AssumeUniversal,
                                              out var dt)
                                ? dt
                                : DateTime.UtcNow,
                Rate = rate
            };
        }

        private async ValueTask<IStockServiceResponse> GetPriceOfStockImpl(string stock, string toCode, CancellationToken cancellationToken = default)
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
            var openRate = result.Series.First().Value.Open;

            if (!decimal.TryParse(openRate,
                                    NumberStyles.Any,
                                    CultureInfo.InvariantCulture,
                                    out var rate))
                throw new AlphaVantageException($"Invalid rate received {openRate}");

            var company = await ((IStockService)this).GetCompany(stock, cancellationToken)
                .ConfigureAwait(false);

            if(company.Currency.Equals(toCode, StringComparison.InvariantCultureIgnoreCase))
            {
                var forexResult = await ((IForexService)this).GetPriceOf(company.Currency, toCode, cancellationToken)
                    .ConfigureAwait(false);

                rate *= forexResult.Rate;
            }

            return new AlphaVantageStockServiceResponse
            {
                Stock = stock,
                To = toCode,
                Timestamp = DateTime.TryParse(result.MetaData.LastRefreshed,
                                              CultureInfo.InvariantCulture,
                                              DateTimeStyles.AssumeUniversal,
                                              out var dt)
                                ? dt
                                : DateTime.UtcNow,
                Rate = rate
            };
        }

        private async ValueTask<IStockCompanyResponse> GetCompanyImpl(string company, CancellationToken cancellationToken = default)
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

        private async Task<T> GetResult<T>(object queryString, CancellationToken token = default)
        {
            var ub = new UriBuilder("https", "www.alphavantage.co")
            {
                Path = "query",
                Query = queryString.AsQueryString()
            };

            var response = await _httpClient.GetAsync(ub.Uri, token)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new AlphaVantageException($"Fail to get response from forex service {(int)response.StatusCode}");

            await using var content = await response.Content.ReadAsStreamAsync(token)
                .ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<T>(content, cancellationToken: token)
                .ConfigureAwait(false);
        }
    }
}
