using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;
using System;

namespace Kubera.Application.Features.Commands.UpdateTransaction.V1
{
    public class UpdateTransactionCommand : IRequest<Result>
    {
        public Guid Id { get; set; }

        public TransactionUpdateModel Input { get; set; }
    }
}
