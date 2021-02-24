using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;
using System;

namespace Kubera.Application.Features.Commands.UpdateAsset.V1
{
    public class UpdateAssetCommand : IRequest<IResult>
    {
        public Guid Id { get; set; }

        public AssetUpdateModel Input { get; set; }
    }
}
