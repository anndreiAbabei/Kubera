using Kubera.Application.Common.Models;
using System.Collections.Generic;
using System;
using Kubera.Application.Common.Caching;

namespace Kubera.Application.Features.Queries.GetAssetsTotal.V1
{
    public class GetAssetsTotalQuery : CacheableRequest<IEnumerable<AssetTotalModel>>
    {
        internal override CacheRegion CacheRegion => CacheRegion.Assets | CacheRegion.Transactions;

        public Guid CurrencyId { get; set; }
    }
}
