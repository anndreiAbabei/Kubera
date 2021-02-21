using System.Collections.Generic;
using Kubera.Application.Common.Models;
using Kubera.General.Models;
using Kubera.Application.Common;

namespace Kubera.Application.Features.Queries.GetGroupedTransactions.V1
{
    public class GetGroupedTransactionsQuery : CacheableRequest<IEnumerable<GroupedTransactionsModel>>
    {
        public Order? Order { get; set; }
    }
}