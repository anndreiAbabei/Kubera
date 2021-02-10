using Kubera.Data.Entities;
using Kubera.Data.Store;
using Kubera.General.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Business.Repository
{
    public class CurrencyRepository : CrudRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(ICurrencyStore store) 
            : base(store)
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
