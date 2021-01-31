using Kubera.Data.Entities;
using Kubera.Data.Store.Base;
using Kubera.General.Store;

namespace Kubera.Data.Store
{
    public class CurrencyStore : BaseDbCrudStore<Currency>, ICurrencyStore
    {
        public CurrencyStore(IApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }
    }

    public interface ICurrencyStore : ICrudStore<Currency>
    {
    }
}
