using CSharpFunctionalExtensions;
using MediatR;
using System;

namespace Kubera.Application.Features.Commands.DeleteGroup.V1
{
    public class DeleteGroupCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
