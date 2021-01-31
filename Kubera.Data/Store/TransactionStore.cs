using Kubera.Data.Entities;
using Kubera.Data.Store.Base;
using Kubera.General.Store;

namespace Kubera.Data.Store
{
    public class TransactionStore : BaseDbCrudStore<Transaction>, ITransactionStore
    {
        public TransactionStore(IApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }
    }

    public interface ITransactionStore : ICrudStore<Transaction>
    {
    }
}
