using Kubera.Data.Entities;
using Kubera.Data.Store.Base;
using Kubera.General.Store;

namespace Kubera.Data.Store
{
    public class AssetStore : BaseDbCrudStore<Asset>, IAssetStore
    {
        public AssetStore(IApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }
    }

    public interface IAssetStore : ICrudStore<Asset>
    {
    }
}
