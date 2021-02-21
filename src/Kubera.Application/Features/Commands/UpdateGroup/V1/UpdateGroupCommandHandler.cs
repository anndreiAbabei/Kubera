using CSharpFunctionalExtensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.General.Extensions;
using Kubera.General.Services;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Commands.UpdateGroup.V1
{
    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, IResult>
    {
        private readonly IUserCacheService _userCacheService;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserIdAccesor _userIdAccesor;

        public UpdateGroupCommandHandler(IUserCacheService userCacheService, 
            IGroupRepository groupRepository, 
            IUserIdAccesor userIdAccesor)
        {
            _userCacheService = userCacheService;
            _groupRepository = groupRepository;
            _userIdAccesor = userIdAccesor;
        }

        public async Task<IResult> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
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

            _userCacheService.RemoveAll<GroupModel>();
            _userCacheService.RemoveAll<GroupTotalModel>();
            _userCacheService.RemoveAll<IEnumerable<GroupModel>>();
            _userCacheService.RemoveAll<IEnumerable<GroupTotalModel>>();

            return Result.Success();
        }
    }
}
