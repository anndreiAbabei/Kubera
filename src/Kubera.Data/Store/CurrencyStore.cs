using Kubera.Data.Entities;
using Kubera.Data.Store.Base;
using Kubera.General.Store;
using Microsoft.Extensions.Logging;

namespace Kubera.Data.Store
{
    public class CurrencyStore : BaseDbCrudStore<Currency>, ICurrencyStore
    {
        public CurrencyStore(IApplicationDbContext applicationDbContext, ILogger<CurrencyStore> logger)
            : base(applicationDbContext, logger)
        {
        }
    }

    public interface ICurrencyStore : ICrudStore<Currency>
    {
    }
}
