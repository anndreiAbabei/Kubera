using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using Kubera.General.Extensions;
using Kubera.General.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Queries.GetGroup.V1
{
    public class GetGroupQueryHandler : IRequestHandler<GetGroupQuery, Result<GroupModel>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;
        private readonly IUserIdAccesor _userIdAccesor;

        public GetGroupQueryHandler(IGroupRepository groupRepository, IMapper mapper, IUserIdAccesor userIdAccesor)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
            _userIdAccesor = userIdAccesor;
        }

        public async Task<Result<GroupModel>> Handle(GetGroupQuery request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetById(request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (group == null)
                return Result.Failure<GroupModel>(ErrorCodes.NotFound);

            if (group.OwnerId != _userIdAccesor.Id)
                return Result.Failure<GroupModel>(ErrorCodes.Forbid);

            return _mapper.Map<Group, GroupModel>(group);
        }
    }
}
