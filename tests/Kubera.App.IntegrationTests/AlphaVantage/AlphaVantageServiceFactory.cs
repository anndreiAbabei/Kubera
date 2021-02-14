using System;
using System.Net.Http;
using Kubera.Business.Services;
using Kubera.General.Services;

namespace Kubera.App.IntegrationTests.AlphaVantage
{
    public class AlphaVantageServiceFactory : IntegrationTestingFactory, IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool disposedValue;

        public IForexService Forex { get; }

        public AlphaVantageServiceFactory()
        {
            _httpClient = new HttpClient();
            var service = new AlphaVantageService(_httpClient, AppSettings, CacheService);
            Forex = service;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                }

                disposedValue = true;
            }
        }

        ~AlphaVantageServiceFactory()
        {
             Dispose(false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
