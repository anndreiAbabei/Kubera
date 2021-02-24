using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Extensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using Kubera.General.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Queries.GetAllGroups.V1
{
    public class GetAllGroupsQueryHandler : CachingHandler<GetAllGroupsQuery, IEnumerable<GroupModel>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GetAllGroupsQueryHandler(IUserCacheService cacheService,
            IGroupRepository groupRepository,
            IMapper mapper)
            : base(cacheService)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;

            cacheService.SetAbsoluteExpiration(DateTimeOffset.Now.AddDays(1));
        }

        protected override async ValueTask<IResult<IEnumerable<GroupModel>>> HandleImpl(GetAllGroupsQuery request, CancellationToken cancellationToken)
        {
            var groups = await _groupRepository.GetAll()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return groups.Select(_mapper.Map<Group, GroupModel>)
                         .AsResult();
        }
    }
}
