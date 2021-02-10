using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;
using System;

namespace Kubera.Application.Features.Commands.UpdateGroup.V1
{
    public class UpdateGroupCommand : IRequest<Result>
    {
        public Guid Id { get; set; }

        public GroupUpdateModel Input { get; set; }
    }
}
