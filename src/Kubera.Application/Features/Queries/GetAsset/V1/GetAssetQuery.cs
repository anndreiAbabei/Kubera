using Kubera.Application.Common;
using Kubera.Application.Common.Models;
using System;

namespace Kubera.Application.Features.Queries.GetAsset.V1
{
    public class GetAssetQuery : CacheableRequest<AssetModel>
    {
        public Guid Id { get; set; }
    }
}
