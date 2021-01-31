using Kubera.Data.Entities;
using Kubera.Data.Store;
using Kubera.General.Repository;
using Kubera.General.Services;
using System.Linq;

namespace Kubera.Business.Repository
{
    public class TransactionRepository : CrudRepository<Transaction>, ITransactionRepository
    {
        private readonly IUserIdAccesor _userIdAccesor;

        public TransactionRepository(ITransactionStore store, IUserIdAccesor userIdAccesor) 
            : base(store)
        {
            _userIdAccesor = userIdAccesor;
        }

        public override IQueryable<Transaction> GetAll()
        {
            var user = _userIdAccesor.Id;

            return base.GetAll().Where(a => a.OwnerId == user);
        }
    }

    public interface ITransactionRepository : ICrudRepository<Transaction>
    {
    }
}
