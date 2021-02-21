using Kubera.Application.Common;
using Kubera.Application.Common.Models;
using System;
using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetGroupTotals.V1
{
    public class GetGroupTotalQuery : CacheableRequest<IEnumerable<GroupTotalModel>>
    {
        public Guid CurrencyId { get; set; }
    }
}
