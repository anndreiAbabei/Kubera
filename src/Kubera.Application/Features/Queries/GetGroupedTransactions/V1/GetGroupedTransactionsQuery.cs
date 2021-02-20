using System.Collections.Generic;
using CSharpFunctionalExtensions;
using MediatR;
using Kubera.Application.Common.Models;
using Kubera.General.Models;

namespace Kubera.Application.Features.Queries.GetGroupedTransactions.V1 
{
    public class GetGroupedTransactionsQuery : IRequest<IResult<IEnumerable<GroupedTransactionsModel>>>
    {
        public Order? Order { get; set; }
    }
}