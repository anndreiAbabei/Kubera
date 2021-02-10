using Kubera.Business.Services;
using Kubera.General;
using Kubera.General.Services;

namespace Kubera.App
{
    public class AppSettings : IAppSettings
    {
        public CacheOptions CacheOptions { get; set; }

        ICacheOptions IAppSettings.CacheOptions => CacheOptions;
    }
}
