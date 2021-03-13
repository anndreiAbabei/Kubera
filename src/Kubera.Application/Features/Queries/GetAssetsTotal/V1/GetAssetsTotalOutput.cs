using Kubera.Application.Common.Models;
using Kubera.Application.Features.Models;
using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetAssetsTotal.V1
{
    public class GetAssetsTotalOutput : CollectionOutputModel
    {
        public IEnumerable<AssetTotalModel> Assets { get; set; }

        public decimal Total { get; set; }

        public decimal TotalNow { get; set; }

        public float Increase { get; set; }
    }
}
