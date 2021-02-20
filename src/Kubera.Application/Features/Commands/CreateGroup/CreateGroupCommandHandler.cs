﻿using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Extensions;
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
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, IResult<GroupModel>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;
        private readonly IUserIdAccesor _userIdAccesor;

        public CreateGroupCommandHandler(IGroupRepository groupRepository, IMapper mapper, IUserIdAccesor userIdAccesor)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
            _userIdAccesor = userIdAccesor;
        }

        public async Task<IResult<GroupModel>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
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

            return _mapper.Map<Group, GroupModel>(group).AsResult();
        }
    }
}
