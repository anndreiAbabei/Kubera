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

namespace Kubera.Application.Features.Queries.GetAsset.V1
{
    public class GetAssetQueryHandler : CachingHandler<GetAssetQuery, AssetModel>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;
        private readonly IUserIdAccesor _userIdAccesor;

        public GetAssetQueryHandler(IUserCacheService cacheService, 
            IAssetRepository assetRepository, 
            IMapper mapper, 
            IUserIdAccesor userIdAccesor)
            : base(cacheService)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;
            _userIdAccesor = userIdAccesor;

            cacheService.SetAbsoluteExpiration(DateTimeOffset.Now.AddDays(1));
        }

        protected override async ValueTask<IResult<AssetModel>> HandleImpl(GetAssetQuery request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetById(request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (asset == null)
                return Result.Failure<AssetModel>(ErrorCodes.NotFound);

            if (asset.OwnerId != _userIdAccesor.Id)
                return Result.Failure<AssetModel>(ErrorCodes.Forbid);

            return _mapper.Map<Asset, AssetModel>(asset)
                .AsResult();
        }

        protected override string GenerateKey(GetAssetQuery request) => $"{base.GenerateKey(request)}.{request.Id}";
    }
}
