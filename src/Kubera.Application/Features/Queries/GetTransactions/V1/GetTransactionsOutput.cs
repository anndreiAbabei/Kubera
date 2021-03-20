using Kubera.Application.Common.Models;
using Kubera.General.Models;
using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetTransactions.V1
{
    public class GetTransactionsOutput
    {
        public IEnumerable<TransactionModel> Transactions { get; init; }
        public IPagingResult Paging { get; init; }
    }
}
