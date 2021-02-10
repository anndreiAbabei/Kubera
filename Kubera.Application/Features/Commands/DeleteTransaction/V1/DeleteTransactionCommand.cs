using CSharpFunctionalExtensions;
using MediatR;
using System;

namespace Kubera.Application.Features.Commands.DeleteTransaction.V1
{
    public class DeleteTransactionCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
