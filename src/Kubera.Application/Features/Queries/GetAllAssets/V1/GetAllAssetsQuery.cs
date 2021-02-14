using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetAllAssets.V1
{
    public class GetAllAssetsQuery : IRequest<Result<IEnumerable<AssetModel>>>
    {
    }
}
