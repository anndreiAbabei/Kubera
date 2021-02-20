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

namespace Kubera.Application.Features.Queries.GetAllAssets.V1
{
    public class GetAllAssetsQueryHandler : IRequestHandler<GetAllAssetsQuery, IResult<IEnumerable<AssetModel>>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public GetAllAssetsQueryHandler(IAssetRepository assetRepository, IMapper mapper)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;
        }

        public async Task<IResult<IEnumerable<AssetModel>>> Handle(GetAllAssetsQuery request, CancellationToken cancellationToken)
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
