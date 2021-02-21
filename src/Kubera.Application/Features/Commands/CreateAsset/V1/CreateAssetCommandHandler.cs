using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Extensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using Kubera.General.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Commands.CreateAsset.V1
{
    public class CreateAssetCommandHandler : IRequestHandler<CreateAssetCommand, IResult<AssetModel>>
    {
        private readonly IUserCacheService _userCacheService;
        private readonly IAssetRepository _assetRepository;
        private readonly IUserIdAccesor _userIdAccesor;
        private readonly IMapper _mapper;

        public CreateAssetCommandHandler(IUserCacheService userCacheService, 
            IAssetRepository assetRepository, 
            IUserIdAccesor userIdAccesor, 
            IMapper mapper)
        {
            _userCacheService = userCacheService;
            _assetRepository = assetRepository;
            _userIdAccesor = userIdAccesor;
            _mapper = mapper;
        }

        public async Task<IResult<AssetModel>> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = new Asset
            {
                Code = request.Input.Code,
                Name = request.Input.Name,
                GroupId = request.Input.GroupId,
                Order = request.Input.Order,
                Icon = request.Input.Icon,
                Symbol = request.Input.Symbol,
                CreatedAt = DateTime.UtcNow,
                OwnerId = _userIdAccesor.Id
            };

            if (await _assetRepository.Exists(asset, cancellationToken).ConfigureAwait(false))
                return Result.Failure<AssetModel>(ErrorCodes.Forbid);

            asset = await _assetRepository.Add(asset, cancellationToken)
                .ConfigureAwait(false);

            _userCacheService.RemoveAll<AssetModel>();
            _userCacheService.RemoveAll<GroupTotalModel>();
            _userCacheService.RemoveAll<IEnumerable<AssetModel>>(); 
            _userCacheService.RemoveAll<IEnumerable<GroupTotalModel>>(); 

            return _mapper.Map<Asset, AssetModel>(asset).AsResult();
        }
    }
}
