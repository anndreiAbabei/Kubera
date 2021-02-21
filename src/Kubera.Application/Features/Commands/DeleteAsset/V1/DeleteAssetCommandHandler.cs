using CSharpFunctionalExtensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.General.Extensions;
using Kubera.General.Services;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Commands.DeleteAsset.V1
{
    public class DeleteAssetCommandHandler : IRequestHandler<DeleteAssetCommand, IResult>
    {
        private readonly IUserCacheService _userCacheService;
        private readonly IAssetRepository _assetRepository;
        private readonly IUserIdAccesor _userIdAccesor;

        public DeleteAssetCommandHandler(IUserCacheService userCacheService, 
            IAssetRepository assetRepository, 
            IUserIdAccesor userIdAccesor)
        {
            _userCacheService = userCacheService;
            _assetRepository = assetRepository;
            _userIdAccesor = userIdAccesor;
        }

        public async Task<IResult> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetById(request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (asset == null)
                return Result.Failure(ErrorCodes.NotFound);

            if (asset.OwnerId != _userIdAccesor.Id)
                return Result.Failure(ErrorCodes.Forbid);

            await _assetRepository.Delete(asset.Id, cancellationToken)
                .ConfigureAwait(false);

            _userCacheService.RemoveAll<GroupModel>();
            _userCacheService.RemoveAll<GroupTotalModel>();
            _userCacheService.RemoveAll<IEnumerable<GroupModel>>();
            _userCacheService.RemoveAll<IEnumerable<GroupTotalModel>>();

            return Result.Success();
        }
    }
}
