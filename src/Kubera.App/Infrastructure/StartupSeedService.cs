using System;
using System.Threading;
using System.Threading.Tasks;
using Kubera.App.Data;
using Kubera.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kubera.App.Infrastructure 
{
    public class StartupSeedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public StartupSeedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var serviceScope = _serviceProvider.CreateScope();

            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            var seeder = serviceScope.ServiceProvider.GetService<ISeeder>();

            await context.Database.MigrateAsync(cancellationToken)
                .ConfigureAwait(false);

            await seeder.Seed(cancellationToken)
                .ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}