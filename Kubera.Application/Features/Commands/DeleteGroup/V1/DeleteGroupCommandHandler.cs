using CSharpFunctionalExtensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Services;
using Kubera.General.Extensions;
using Kubera.General.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Commands.DeleteGroup.V1
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Result>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserIdAccesor _userIdAccesor;

        public DeleteGroupCommandHandler(IGroupRepository groupRepository, IUserIdAccesor userIdAccesor)
        {
            _groupRepository = groupRepository;
            _userIdAccesor = userIdAccesor;
        }

        public async Task<Result> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetById(request.Id, cancellationToken);

            if (group == null)
                return Result.Failure(ErrorCodes.NotFound);

            if (group.OwnerId != _userIdAccesor.Id)
                return Result.Failure(ErrorCodes.Forbid);

            await _groupRepository.Delete(group.Id, cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }
    }
}
