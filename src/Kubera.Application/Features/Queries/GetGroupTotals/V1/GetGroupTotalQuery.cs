using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetGroupTotals.V1
{
    public class GetGroupTotalQuery : IRequest<IResult<IEnumerable<GroupTotalModel>>>
    {
        public Guid CurrencyId { get; set; }
    }
}
