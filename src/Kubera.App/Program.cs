using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using ConfigurationSubstitution;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kubera.App
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();



        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureLogging(ConfigureLogging)
                       .ConfigureAppConfiguration(ConfigureConfiguration)
                       .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }



        private static void ConfigureLogging(HostBuilderContext ctx, ILoggingBuilder builder)
        {
            builder.AddConsole();
        }



        private static void ConfigureConfiguration(HostBuilderContext ctx, IConfigurationBuilder builder)
        {
            builder.EnableSubstitutions("$(", ")");
            AddUserSecrets(ctx, builder);
        }



        private static void AddUserSecrets(HostBuilderContext ctx, IConfigurationBuilder builder)
        {
            if (ctx.HostingEnvironment.IsDevelopment())
                builder.AddUserSecrets<Program>();
            else if (ctx.HostingEnvironment.IsProduction())
            {
                var builtConfig = builder.Build();

                using var store = new X509Store(StoreLocation.CurrentUser);

                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindByThumbprint,
                                                    builtConfig["AzureADCertThumbprint"], 
                                                    false);

                builder.AddAzureKeyVault(new Uri($"https://{builtConfig["KeyVaultName"]}.vault.azure.net/"),
                                         new ClientCertificateCredential(builtConfig["AzureADDirectoryId"],
                                                                         builtConfig["AzureADApplicationId"],
                                                                         certs.OfType<X509Certificate2>().Single()),
                                         new KeyVaultSecretManager());


                store.Close();
            }
        }
    }
}
