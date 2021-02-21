using Kubera.Application.Common;
using Kubera.Application.Common.Models;
using System;

namespace Kubera.Application.Features.Queries.GetGroup.V1
{
    public class GetGroupQuery : CacheableRequest<GroupModel>
    {
        public Guid Id { get; set; }
    }
}
