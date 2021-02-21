using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Models;
using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetAllAssets.V1
{
    public class GetAllAssetsQuery : CacheableRequest<IEnumerable<AssetModel>>
    {
        internal override CacheRegion CacheRegion => CacheRegion.Assets;
    }
}
