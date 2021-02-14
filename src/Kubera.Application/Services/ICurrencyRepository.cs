using Kubera.Data.Entities;
using Kubera.General.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Services
{
    public interface ICurrencyRepository : IReadOnlyRepository<Currency>, IAddeable<Currency>
    {
        ValueTask<Currency> GetByCode(string code, CancellationToken cancellationToken = default);
    }
}
