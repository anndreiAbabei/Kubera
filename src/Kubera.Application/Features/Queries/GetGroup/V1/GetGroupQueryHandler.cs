using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Extensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using Kubera.General.Extensions;
using Kubera.General.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Queries.GetGroup.V1
{
    public class GetGroupQueryHandler : CachingHandler<GetGroupQuery, GroupModel>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserIdAccesor _userIdAccesor;
        private readonly IMapper _mapper;

        public GetGroupQueryHandler(IUserCacheService cacheService, 
            IGroupRepository groupRepository, 
            IUserIdAccesor userIdAccesor, 
            IMapper mapper)
            : base(cacheService)
        {
            _groupRepository = groupRepository;
            _userIdAccesor = userIdAccesor;
            _mapper = mapper;

            cacheService.SetAbsoluteExpiration(DateTimeOffset.Now.AddDays(1));
        }

        protected override async ValueTask<IResult<GroupModel>> HandleImpl(GetGroupQuery request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetById(request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (group == null)
                return Result.Failure<GroupModel>(ErrorCodes.NotFound);

            if (group.OwnerId != _userIdAccesor.Id)
                return Result.Failure<GroupModel>(ErrorCodes.Forbid);

            return _mapper.Map<Group, GroupModel>(group)
                .AsResult();
        }

        protected override string GenerateKey(GetGroupQuery request) => $"{base.GenerateKey(request)}.{request.Id}";
    }
}
