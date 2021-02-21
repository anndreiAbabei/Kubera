using Kubera.Application.Common;
using Kubera.Application.Common.Models;
using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetAllGroups.V1
{
    public class GetAllGroupsQuery : CacheableRequest<IEnumerable<GroupModel>>
    {
    }
}
