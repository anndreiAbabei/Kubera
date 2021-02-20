using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;
using System;

namespace Kubera.Application.Features.Queries.GetTransaction.V1
{
    public class GetTransactionQuery : IRequest<IResult<TransactionModel>>
    {
        public Guid Id { get; set; }
    }
}
