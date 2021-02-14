using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Extensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Queries.GetAllGroups.V1
{
    public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, Result<IEnumerable<GroupModel>>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GetAllGroupsQueryHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<GroupModel>>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
        {
            var groups = await _groupRepository.GetAll()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return groups.Select(_mapper.Map<Group, GroupModel>)
                         .AsResult();
        }
    }
}
