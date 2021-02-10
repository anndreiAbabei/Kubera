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

namespace Kubera.Application.Features.Queries.GetAsset.V1
{
    public class GetAssetQueryHandler : IRequestHandler<GetAssetQuery, Result<AssetModel>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;
        private readonly IUserIdAccesor _userIdAccesor;

        public GetAssetQueryHandler(IAssetRepository assetRepository, IMapper mapper, IUserIdAccesor userIdAccesor)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;
            _userIdAccesor = userIdAccesor;
        }

        public async Task<Result<AssetModel>> Handle(GetAssetQuery request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetById(request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (asset == null)
                return Result.Failure<AssetModel>(ErrorCodes.NotFound);

            if (asset.OwnerId != _userIdAccesor.Id)
                return Result.Failure<AssetModel>(ErrorCodes.Forbid);

            return _mapper.Map<Asset, AssetModel>(asset);

        }
    }
}
