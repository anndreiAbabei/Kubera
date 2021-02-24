using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Models;
using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetAllGroups.V1
{
    public class GetAllGroupsQuery : CacheableRequest<IEnumerable<GroupModel>>
    {
        internal override CacheRegion CacheRegion => CacheRegion.Groups;
    }
}
