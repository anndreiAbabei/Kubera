using Kubera.Data.Entities;
using Kubera.General.Repository;

namespace Kubera.Application.Services
{
    public interface ITransactionRepository : ICrudRepository<Transaction>
    {
    }
}
