using Kubera.Data.Entities;
using Kubera.Data.Store.Base;
using Kubera.General.Store;
using Microsoft.Extensions.Logging;

namespace Kubera.Data.Store
{
    public class GroupStore : BaseDbCrudStore<Group>, IGroupStore
    {
        public GroupStore(IApplicationDbContext applicationDbContext, ILogger<GroupStore> logger) 
            : base(applicationDbContext, logger)
        {
        }
    }

    public interface IGroupStore : ICrudStore<Group>
    {
    }
}
