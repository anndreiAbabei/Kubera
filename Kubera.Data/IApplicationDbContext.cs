using Kubera.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Data
{
    public interface IApplicationDbContext
    {
        DbSet<ApplicationUser> Users { get; }

        DbSet<Group> Groups { get; }

        DbSet<Currency> Currencies { get; }

        DbSet<Asset> Assets { get; }

        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
