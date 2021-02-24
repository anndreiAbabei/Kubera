using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Models;
using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetCurrencies.V1
{
    public class GetCurrenciesQuery : CacheableRequest<IEnumerable<CurrencyModel>>
    {
        internal override CacheRegion CacheRegion => CacheRegion.Currencies;
    }
}
