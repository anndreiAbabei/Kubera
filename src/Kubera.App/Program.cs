using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using ConfigurationSubstitution;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Kubera.App
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((ctx, builder) =>
                {
                    builder.EnableSubstitutions("$(", ")");
                    AddUserSecrets(ctx, builder);
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

        private static void AddUserSecrets(HostBuilderContext ctx, IConfigurationBuilder builder)
        {
            if (ctx.HostingEnvironment.IsDevelopment())
                builder.AddUserSecrets<Program>();
            else
            {
                var root = builder.Build();
                var vaultName = root["KeyVault:Name"];
                var appId = root["KeyVault:ADApplicationId"];
                var directoryId = root["KeyVault:ADDirectoryId"];
                var cert = GetApplicationCertificate(root);

                var uri = new Uri($"https://{vaultName}.vault.azure.net/");
                var credential = new ClientCertificateCredential(directoryId, appId, cert);
                var manager = new KeyVaultSecretManager();

                builder.AddAzureKeyVault(uri, credential, manager);
            }
        }

        internal static X509Certificate2 GetApplicationCertificate(IConfiguration configuration)
        {
            var thumbprint = configuration["Application:Certificate"];

            if (string.IsNullOrEmpty(thumbprint))
                return null;

            using var store = new X509Store(StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                return store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false)
                                .OfType<X509Certificate2>()
                                .Single();
            }
            finally
            {
                store.Close();
            }
        }
    }
}
