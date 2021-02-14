using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;
using System;

namespace Kubera.Application.Features.Queries.GetGroup.V1
{
    public class GetGroupQuery : IRequest<Result<GroupModel>>
    {
        public Guid Id { get; set; }
    }
}
