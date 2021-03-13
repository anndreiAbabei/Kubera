using Kubera.Application.Common.Models;
using Kubera.Application.Features.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kubera.Application.Features.Queries.GetAssetsTotal.V1
{
    [DebuggerDisplay("Count: {Count} assets, Total: {Total}, TotalNow: {TotalNow}, Increase: {Increase}")]
    public class GetAssetsTotalOutput : CollectionOutputModel
    {
        public IEnumerable<AssetTotalModel> Assets { get; set; }

        public decimal Total { get; set; }

        public decimal TotalNow { get; set; }

        public float Increase { get; set; }
    }
}
