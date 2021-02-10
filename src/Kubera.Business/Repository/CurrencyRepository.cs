using Kubera.Data.Entities;
using Kubera.Data.Store;
using Kubera.General.Repository;
using Kubera.General.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Business.Repository
{
    public class CurrencyRepository : CachedCrudRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(ICurrencyStore store,
            ICacheService cacheService,
            ICacheOptions options)
            : base(store, cacheService, options)
        {
        }

        public async ValueTask<Currency> GetByCode(string code, CancellationToken cancellationToken = default)
        {
            var query = await GetAll(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return await query.FirstOrDefaultAsync(c => c.Code == code, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    public interface ICurrencyRepository : IReadOnlyRepository<Currency>, IAddeable<Currency>
    {
        ValueTask<Currency> GetByCode(string code, CancellationToken cancellationToken = default);
    }
}
