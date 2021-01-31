using Kubera.Data.Entities;
using Kubera.Data.Store;
using Kubera.General.Repository;
using Kubera.General.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Business.Repository
{
    public class AssetRepository : CrudRepository<Asset>, IAssetRepository
    {
        private readonly IUserIdAccesor _userIdAccesor;

        public AssetRepository(IAssetStore store, IUserIdAccesor userIdAccesor) 
            : base(store)
        {
            _userIdAccesor = userIdAccesor;
        }

        public async ValueTask<bool> Exists(Asset asset, CancellationToken cancellationToken = default)
        {
            return await GetAll()
                .AnyAsync(g => g.Id != asset.Id &&
                               (g.OwnerId == null || g.OwnerId == asset.OwnerId) &&
                               (g.Code == asset.Code || g.Name == asset.Name),
                         cancellationToken)
                .ConfigureAwait(false);
        }

        public async ValueTask<Asset> GetByCode(string code, CancellationToken cancellationToken = default)
        {
            return await GetAll().FirstOrDefaultAsync(a => a.Code == code, cancellationToken)
                .ConfigureAwait(false);
        }

        public override IQueryable<Asset> GetAll()
        {
            var user = _userIdAccesor.Id;

            return base.GetAll().Where(a => a.OwnerId == null || a.OwnerId == user);
        }
    }

    public interface IAssetRepository : ICrudRepository<Asset>
    {
        ValueTask<Asset> GetByCode(string code, CancellationToken cancellationToken = default);
        ValueTask<bool> Exists(Asset asset, CancellationToken cancellationToken = default);
    }
}
