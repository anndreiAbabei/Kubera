using Kubera.App.Infrastructure.Authorization;
using Kubera.App.Infrastructure.Cors;
using Kubera.App.Infrastructure.Mail;
using Kubera.App.Static;
using Kubera.Business.Services;
using Kubera.General;
using Kubera.General.Services;
using Kubera.General.Settings;

namespace Kubera.App
{
    public class AppSettings : IAppSettings
    {
        public string AppName => Resources.AppName;
        public string ApiName => Resources.AppName;

        public string DatabaseConnectionString { get; set; }

        public string AlphaVantageApiKey { get; set; }

        public int DatabaseRetries { get; set; }

        public CacheOptions CacheOptions { get; set; }

        public AutorisationSettings Autorisation { get; set; }

        public CorsOptions Cors { get; set; }

        public MailOptions Mail { get; set; }

        ICacheOptions IAppSettings.CacheOptions => CacheOptions;

        IAutorisationSettings IAppSettings.Autorisation => Autorisation;

        ICorsOptions IAppSettings.Cors => Cors;

        IMailOptions IAppSettings.Mail => Mail;
    }
}
