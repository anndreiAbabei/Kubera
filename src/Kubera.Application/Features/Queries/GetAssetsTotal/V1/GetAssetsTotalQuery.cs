using Kubera.Application.Common.Models;
using System.Collections.Generic;
using System;
using Kubera.Application.Common;

namespace Kubera.Application.Features.Queries.GetAssetsTotal.V1
{
    public class GetAssetsTotalQuery : CacheableRequest<IEnumerable<AssetTotalModel>>
    {
        public Guid CurrencyId { get; set; }
    }
}
