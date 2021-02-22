using Kubera.Data.Entities;
using Kubera.Data.Store.Base;
using Kubera.General.Store;
using Microsoft.Extensions.Logging;

namespace Kubera.Data.Store
{
    public class AssetStore : BaseDbCrudStore<Asset>, IAssetStore
    {
        public AssetStore(IApplicationDbContext applicationDbContext, ILogger<AssetStore> logger)
            : base(applicationDbContext, logger)
        {
        }
    }

    public interface IAssetStore : ICrudStore<Asset>
    {
    }
}
