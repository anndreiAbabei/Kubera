using Kubera.Application.Common.Caching;

namespace Kubera.Application.Features.Queries.GetWallets.V1
{
    public class GetWalletsQuery : CacheableRequest<GetWalletsOutput>
    {
        internal override CacheRegion CacheRegion => CacheRegion.Transactions;

    }
}
