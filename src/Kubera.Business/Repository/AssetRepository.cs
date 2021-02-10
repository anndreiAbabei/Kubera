using Kubera.Data.Entities;
using Kubera.Data.Store;
using Kubera.General.Models;
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
            var query = await GetAll(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return await query.AnyAsync(g => g.Id != asset.Id &&
                                             (g.OwnerId == null || g.OwnerId == asset.OwnerId) &&
                                             (g.Code == asset.Code || g.Name == asset.Name),
                                     cancellationToken)
                .ConfigureAwait(false);
        }

        public async ValueTask<Asset> GetByCode(string code, CancellationToken cancellationToken = default)
        {
            var query = await GetAll(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return await query.FirstOrDefaultAsync(a => a.Code == code, cancellationToken)
                .ConfigureAwait(false);
        }

        public override async ValueTask<IQueryable<Asset>> GetAll(IPaging paging = null, IDateFilter dateFilter = null, CancellationToken cancellationToken = default)
        {
            var user = _userIdAccesor.Id;
            var query = await base.GetAll(paging, dateFilter, cancellationToken)
                .ConfigureAwait(false);

            return query.Where(a => string.IsNullOrEmpty(a.OwnerId) || a.OwnerId == user);
        }
    }

    public interface IAssetRepository : ICrudRepository<Asset>
    {
        ValueTask<Asset> GetByCode(string code, CancellationToken cancellationToken = default);
        ValueTask<bool> Exists(Asset asset, CancellationToken cancellationToken = default);
    }
}
