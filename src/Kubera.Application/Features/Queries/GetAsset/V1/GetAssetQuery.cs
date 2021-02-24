using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Models;
using System;

namespace Kubera.Application.Features.Queries.GetAsset.V1
{
    public class GetAssetQuery : CacheableRequest<AssetModel>
    {
        internal override CacheRegion CacheRegion => CacheRegion.Assets;

        public Guid Id { get; set; }
    }
}
