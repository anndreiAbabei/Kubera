using Kubera.General.Services;

namespace Kubera.General
{
    public interface IAppSettings
    {
        ICacheOptions CacheOptions { get; }
        string AlphaVantageApiKey { get; }
    }
}
