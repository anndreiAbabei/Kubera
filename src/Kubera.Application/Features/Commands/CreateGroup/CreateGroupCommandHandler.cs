using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using Kubera.General.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Commands.CreateGroup
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Result<GroupModel>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public CreateGroupCommandHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<Result<GroupModel>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = new Group
            {
                Code = request.Input.Code,
                Name = request.Input.Name,
                CreatedAt = DateTime.UtcNow,
                OwnerId = _userIdAccesor.Id
            };

            if (await _groupRepository.Exists(group, cancellationToken).ConfigureAwait(false))
                return Result.Failure<GroupModel>(ErrorCodes.Forbid);

            group = await _groupRepository.Add(group, cancellationToken)
                .ConfigureAwait(false);

            return _mapper.Map<Group, GroupModel>(group);
        }
    }
}
