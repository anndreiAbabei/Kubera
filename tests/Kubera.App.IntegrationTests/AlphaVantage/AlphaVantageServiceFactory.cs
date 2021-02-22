using System;
using System.Net.Http;
using Kubera.Business.Services;
using Kubera.General.Services;

namespace Kubera.App.IntegrationTests.AlphaVantage
{
    public sealed class AlphaVantageServiceFactory : IntegrationTestingFactory, IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposedValue;

        public IForexService Forex { get; }

        public AlphaVantageServiceFactory()
        {
            _httpClient = new HttpClient();
            var service = new AlphaVantageService(_httpClient, 
                                                  AppSettings, 
                                                  CacheService,
                                                  Logger<AlphaVantageService>());
            Forex = service;
        }

        private void Dispose(bool disposing)
        {
            if (_disposedValue)
                return;

            if (disposing)
                _httpClient.Dispose();

            _disposedValue = true;
        }

        ~AlphaVantageServiceFactory()
        {
             Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
