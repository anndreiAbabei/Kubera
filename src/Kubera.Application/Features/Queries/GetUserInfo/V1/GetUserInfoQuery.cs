using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Queries.GetUserInfo.V1
{
    public class GetUserInfoQuery : CacheableRequest<UserInfoModel>
    {
        internal override CacheRegion CacheRegion => CacheRegion.Users;
    }
}
