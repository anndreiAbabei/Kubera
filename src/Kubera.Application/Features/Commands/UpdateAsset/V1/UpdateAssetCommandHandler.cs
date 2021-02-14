using CSharpFunctionalExtensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Services;
using Kubera.General.Extensions;
using Kubera.General.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Commands.UpdateAsset.V1
{
    public class UpdateAssetCommandHandler : IRequestHandler<UpdateAssetCommand, Result>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IUserIdAccesor _userIdAccesor;

        public UpdateAssetCommandHandler(IAssetRepository assetRepository, IUserIdAccesor userIdAccesor)
        {
            _assetRepository = assetRepository;
            _userIdAccesor = userIdAccesor;
        }

        public async Task<Result> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetById(request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (asset == null)
                return Result.Failure(ErrorCodes.NotFound);

            if (asset.OwnerId != _userIdAccesor.Id)
                return Result.Failure(ErrorCodes.Forbid);

            if (await _assetRepository.Exists(asset, cancellationToken).ConfigureAwait(false))
                return Result.Failure(ErrorCodes.Conflict);

            asset.Code = request.Input.Code;
            asset.Name = request.Input.Name;
            asset.Symbol = request.Input.Symbol;
            asset.Icon = request.Input.Icon;
            asset.GroupId = request.Input.GroupId;
            asset.Order = request.Input.Order;

            await _assetRepository.Update(asset, cancellationToken)
                    .ConfigureAwait(false);

            return Result.Success();
        }
    }
}
