using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Models;
using System;

namespace Kubera.Application.Features.Queries.GetTransaction.V1
{
    public class GetTransactionQuery : CacheableRequest<TransactionModel>
    {
        internal override CacheRegion CacheRegion => CacheRegion.Transactions;

        public Guid Id { get; set; }
    }
}
