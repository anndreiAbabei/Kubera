using CSharpFunctionalExtensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Services;
using Kubera.General.Extensions;
using Kubera.General.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Commands.UpdateGroup.V1
{
    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, Result>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserIdAccesor _userIdAccesor;

        public UpdateGroupCommandHandler(IGroupRepository groupRepository, IUserIdAccesor userIdAccesor)
        {
            _groupRepository = groupRepository;
            _userIdAccesor = userIdAccesor;
        }

        public async Task<Result> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetById(request.Id, cancellationToken).ConfigureAwait(false);

            if (group == null)
                return Result.Failure(ErrorCodes.NotFound);

            if (group.OwnerId != _userIdAccesor.Id)
                return Result.Failure(ErrorCodes.Forbid);

            if (await _groupRepository.Exists(group, cancellationToken).ConfigureAwait(false))
                return Result.Failure(ErrorCodes.Conflict);

            group.Code = request.Input.Code;
            group.Name = request.Input.Name;

            await _groupRepository.Update(group, cancellationToken)
                    .ConfigureAwait(false);

            return Result.Success();
        }
    }
}
