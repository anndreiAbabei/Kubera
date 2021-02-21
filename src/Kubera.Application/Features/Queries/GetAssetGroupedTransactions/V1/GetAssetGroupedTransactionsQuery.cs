using System.Collections.Generic;
using Kubera.Application.Common.Models;
using Kubera.General.Models;
using Kubera.Application.Common.Caching;

namespace Kubera.Application.Features.Queries.GetAssetGroupedTransactions.V1
{
    public class GetAssetGroupedTransactionsQuery : CacheableRequest<IEnumerable<GroupedTransactionsModel>>
    {
        internal override CacheRegion CacheRegion => CacheRegion.Assets | CacheRegion.Transactions;

        public Order? Order { get; set; }
    }
}