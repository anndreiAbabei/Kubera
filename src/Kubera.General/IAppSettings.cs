using Kubera.General.Services;
using Kubera.General.Settings;

namespace Kubera.General
{
    public interface IAppSettings
    {
        string AlphaVantageApiKey { get; }
        string DatabaseConnectionString { get; }
        int DatabaseRetries { get; }
        ICacheOptions CacheOptions { get; }
        IAutorisationSettings Autorisation { get; }
        ICorsOptions Cors { get; }
    }
}
