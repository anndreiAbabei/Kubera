using Kubera.Application.Common.Models;
using Kubera.Application.Features.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kubera.Application.Features.Queries.GetGroupTotals.V1
{
    [DebuggerDisplay("Count: {Count} assets, Total: {Total}, TotalNow: {TotalNow}, Increase: {Increase}")]
    public class GetGroupTotalOutput : CollectionOutputModel
    {
        public IEnumerable<GroupTotalModel> Groups { get; set; }

        public decimal Total { get; set; }

        public decimal TotalNow { get; set; }

        public float Increase { get; set; }
    }
}
