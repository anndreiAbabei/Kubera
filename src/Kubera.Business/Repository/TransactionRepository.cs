using Kubera.Data.Entities;
using Kubera.Data.Store;
using Kubera.General.Models;
using Kubera.General.Repository;
using Kubera.General.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Business.Repository
{
    public class TransactionRepository : CachedCrudRepository<Transaction>, ITransactionRepository
    {
        private readonly IUserIdAccesor _userIdAccesor;

        public TransactionRepository(ITransactionStore store,
            ICacheService cacheService,
            IUserIdAccesor userIdAccesor,
            ICacheOptions options)
            : base(store, cacheService, options)
        {
            _userIdAccesor = userIdAccesor;
        }

        public override async ValueTask<IQueryable<Transaction>> GetAll(IPaging paging = null, IDateFilter dateFilter = null, CancellationToken cancellationToken = default)
        {
            var user = _userIdAccesor.Id;
            var query = await base.GetAll(paging, dateFilter, cancellationToken)
                .ConfigureAwait(false);

            return query.Where(a => a.OwnerId == user);
        }
    }

    public interface ITransactionRepository : ICrudRepository<Transaction>
    {
    }
}
