using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;

namespace Kubera.Application.Features.Commands.CreateAsset.V1
{
    public class CreateAssetCommand : IRequest<Result<AssetModel>>
    {
        public AssetInputModel Input { get; set; }
    }
}
