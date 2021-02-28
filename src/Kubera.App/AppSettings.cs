using Kubera.Business.Services;
using Kubera.General;
using Kubera.General.Services;

namespace Kubera.App
{
    public class AppSettings : IAppSettings
    {
        public CacheOptions CacheOptions { get; set; }

        public string DatabaseConnectionString { get; set; }

        public string AlphaVantageApiKey { get; set; }

        public int DatabaseRetries { get; set; }

        ICacheOptions IAppSettings.CacheOptions => CacheOptions;
    }
}
