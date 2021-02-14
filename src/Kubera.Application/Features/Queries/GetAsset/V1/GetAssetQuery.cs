using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;
using System;

namespace Kubera.Application.Features.Queries.GetAsset.V1
{
    public class GetAssetQuery : IRequest<Result<AssetModel>>
    {
        public Guid Id { get; set; }
    }
}
