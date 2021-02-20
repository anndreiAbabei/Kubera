using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;
using System.Collections.Generic;
using System;

namespace Kubera.Application.Features.Queries.GetAssetsTotal.V1
{
    public class GetAssetsTotalQuery : IRequest<IResult<IEnumerable<AssetTotalModel>>>
    {
        public Guid CurrencyId { get; set; }
    }
}
