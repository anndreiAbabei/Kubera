using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Models;
using Kubera.General.Models;
using System;
using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetGroupTotals.V1
{
    public class GetGroupTotalQuery : CacheableRequest<IEnumerable<GroupTotalModel>>
    {
        internal override CacheRegion CacheRegion => CacheRegion.Groups | CacheRegion.Transactions;

        public Guid CurrencyId { get; set; }

        public IFilter Filter { get; set; }
    }
}
