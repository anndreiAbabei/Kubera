using Kubera.Data.Entities;
using Kubera.General.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Services
{
    public interface IAssetRepository : ICrudRepository<Asset>
    {
        ValueTask<Asset> GetByCode(string code, CancellationToken cancellationToken = default);
        ValueTask<bool> Exists(Asset asset, CancellationToken cancellationToken = default);
    }
}
