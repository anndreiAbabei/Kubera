using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetAllGroups.V1
{
    public class GetAllGroupsQuery : IRequest<IResult<IEnumerable<GroupModel>>>
    {
    }
}
