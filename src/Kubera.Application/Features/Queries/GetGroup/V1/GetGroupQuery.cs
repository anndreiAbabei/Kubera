using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Models;
using System;

namespace Kubera.Application.Features.Queries.GetGroup.V1
{
    public class GetGroupQuery : CacheableRequest<GroupModel>
    {
        internal override CacheRegion CacheRegion => CacheRegion.Groups;

        public Guid Id { get; set; }
    }
}
