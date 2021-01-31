using Kubera.Data.Entities;
using Kubera.General.Repository;
using Kubera.General.Store;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Business.Repository
{
    public class CurrencyRepository : CrudRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(ICrudStore<Currency> store) 
            : base(store)
        {
        }

        public async ValueTask<Currency> GetByCode(string code, CancellationToken cancellationToken = default)
        {
            return await GetAll().FirstOrDefaultAsync(c => c.Code == code, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    public interface ICurrencyRepository : IReadOnlyRepository<Currency>, IAddeable<Currency>
    {
        ValueTask<Currency> GetByCode(string code, CancellationToken cancellationToken = default);
    }
}
