using Kubera.Application.Common;
using Kubera.Application.Common.Models;
using System;

namespace Kubera.Application.Features.Queries.GetTransaction.V1
{
    public class GetTransactionQuery : CacheableRequest<TransactionModel>
    {
        public Guid Id { get; set; }
    }
}
