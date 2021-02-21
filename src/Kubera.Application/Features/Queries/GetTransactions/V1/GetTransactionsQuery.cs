using Kubera.Application.Common;
using Kubera.General.Models;

namespace Kubera.Application.Features.Queries.GetTransactions.V1
{
    public class GetTransactionsQuery : CacheableRequest<GetTransactionsQueryOutput>
    {
        public IPaging Paging { get; set; }

        public IDateFilter Date { get; set; }

        public Order? Order { get; set; }
    }
}
