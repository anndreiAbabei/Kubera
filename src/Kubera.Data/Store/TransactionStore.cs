using Kubera.Data.Entities;
using Kubera.Data.Store.Base;
using Kubera.General.Store;
using Microsoft.Extensions.Logging;

namespace Kubera.Data.Store
{
    public class TransactionStore : BaseDbCrudStore<Transaction>, ITransactionStore
    {
        public TransactionStore(IApplicationDbContext applicationDbContext, ILogger<TransactionStore> logger)
            : base(applicationDbContext, logger)
        {
        }
    }

    public interface ITransactionStore : ICrudStore<Transaction>
    {
    }
}
