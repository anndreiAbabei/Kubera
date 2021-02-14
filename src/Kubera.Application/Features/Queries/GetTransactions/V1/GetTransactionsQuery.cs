using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using Kubera.General.Models;
using MediatR;
using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetTransactions.V1
{
    public class GetTransactionsQuery : IRequest<Result<IEnumerable<TransactionModel>>>
    {
        public IPaging Paging { get; set; }

        public IDateFilter Date { get; set; }

        public Order? Order { get; set; }
    }
}
