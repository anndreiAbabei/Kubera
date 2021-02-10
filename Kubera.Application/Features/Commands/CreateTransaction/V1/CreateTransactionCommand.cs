using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;

namespace Kubera.Application.Features.Commands.CreateTransaction.V1
{
    public class CreateTransactionCommand : IRequest<Result<TransactionModel>>
    {
        public TransactionInputModel Input { get; set; }
    }
}
