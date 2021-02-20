using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;

namespace Kubera.Application.Features.Commands.CreateGroup
{
    public class CreateGroupCommand : IRequest<IResult<GroupModel>>
    {
        public GroupInputModel Input { get; set; }
    }
}
