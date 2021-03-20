using Microsoft.Extensions.Configuration;
using System;

namespace Kubera.App.Infrastructure.Environment
{
    public static class AppEnvironmentEx
    {
        private const string _developement = "Development";
        private const string _uat = "UAT";
        private const string _production = "Production";


        public static AppEnvironment GetAppEnvironment(this IConfiguration configuration)
        {
            var env = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");
            return env switch
            {
                _developement => AppEnvironment.Developement,
                _uat => AppEnvironment.UAT,
                _production => AppEnvironment.Production,
                _ => throw new InvalidOperationException($"Invalid environment <{env}>")
            };
        }
    }
}
