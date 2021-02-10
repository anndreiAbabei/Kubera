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

namespace Kubera.Application.Features.Commands.CreateAsset.V1
{
    public class CreateAssetCommandHandler : IRequestHandler<CreateAssetCommand, Result<AssetModel>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;
        private readonly IUserIdAccesor _userIdAccesor;

        public CreateAssetCommandHandler(IAssetRepository assetRepository, IMapper mapper, IUserIdAccesor userIdAccesor)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;
            _userIdAccesor = userIdAccesor;
        }

        public async Task<Result<AssetModel>> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
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

            return _mapper.Map<Asset, AssetModel>(asset);
        }
    }
}
