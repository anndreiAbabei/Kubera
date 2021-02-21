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

namespace Kubera.Application.Features.Commands.DeleteGroup.V1
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, IResult>
    {
        private readonly IUserCacheService _userCacheService;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserIdAccesor _userIdAccesor;

        public DeleteGroupCommandHandler(IUserCacheService userCacheService,
            IGroupRepository groupRepository, 
            IUserIdAccesor userIdAccesor)
        {
            _userCacheService = userCacheService;
            _groupRepository = groupRepository;
            _userIdAccesor = userIdAccesor;
        }

        public async Task<IResult> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetById(request.Id, cancellationToken);

            if (group == null)
                return Result.Failure(ErrorCodes.NotFound);

            if (group.OwnerId != _userIdAccesor.Id)
                return Result.Failure(ErrorCodes.Forbid);

            await _groupRepository.Delete(group.Id, cancellationToken)
                .ConfigureAwait(false);

            _userCacheService.RemoveAll<GroupModel>();
            _userCacheService.RemoveAll<GroupTotalModel>();
            _userCacheService.RemoveAll<IEnumerable<GroupModel>>();
            _userCacheService.RemoveAll<IEnumerable<GroupTotalModel>>();

            return Result.Success();
        }
    }
}
