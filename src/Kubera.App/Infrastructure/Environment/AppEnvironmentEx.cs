using Microsoft.Extensions.Configuration;
using System;

namespace Kubera.App.Infrastructure.Environment
{
    public static class AppEnvironmentEx
    {
        private const string Developement = "Development";
        private const string UAT = "UAT";
        private const string Production = "Production";


        public static AppEnvironment GetAppEnvironment(this IConfiguration configuration)
        {
            var env = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");
            return env switch
            {
                Developement => AppEnvironment.Developement,
                UAT => AppEnvironment.UAT,
                Production => AppEnvironment.Production,
                _ => throw new InvalidOperationException($"Invalid environment <{env}>")
            };
        }
    }
}
