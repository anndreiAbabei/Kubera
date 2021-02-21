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

namespace Kubera.Application.Features.Queries.GetAllAssets.V1
{
    public class GetAllAssetsQueryHandler : CachingHandler<GetAllAssetsQuery, IEnumerable<AssetModel>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public GetAllAssetsQueryHandler(IUserCacheService cacheService,
            IAssetRepository assetRepository, 
            IMapper mapper)
            : base(cacheService)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;

            cacheService.SetAbsoluteExpiration(DateTimeOffset.Now.AddDays(1));
        }

        protected override async ValueTask<IResult<IEnumerable<AssetModel>>> HandleImpl(GetAllAssetsQuery request, CancellationToken cancellationToken)
        {
            var asstes = await _assetRepository.GetAll()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            IEnumerable<AssetModel> result = asstes.Select(_mapper.Map<Asset, AssetModel>)
                .ToList();

            return result.AsResult();
        }
    }
}
