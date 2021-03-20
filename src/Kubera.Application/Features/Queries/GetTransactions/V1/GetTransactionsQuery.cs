using Kubera.Application.Common.Caching;
using Kubera.General.Models;

namespace Kubera.Application.Features.Queries.GetTransactions.V1
{
    public class GetTransactionsQuery : CacheableRequest<GetTransactionsOutput>
    {
        internal override CacheRegion CacheRegion => CacheRegion.Transactions;

        public IPaging Paging { get; set; }

        public IFilter Filter { get; set; }

        public Order? Order { get; set; }
    }
}
