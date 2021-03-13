using System.Collections.Generic;
using Kubera.Application.Common.Models;
using Kubera.General.Models;
using Kubera.Application.Common.Caching;

namespace Kubera.Application.Features.Queries.GetAssetGroupedTransactions.V1
{
    public class GetAssetGroupedTransactionsQuery : CacheableRequest<IEnumerable<GroupedTransactionsModel>>
    {
        internal override CacheRegion CacheRegion => CacheRegion.Assets | CacheRegion.Transactions;

        public IFilter Filter { get; set; }

        public Order? Order { get; set; }
    }
}