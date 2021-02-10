using Kubera.Data.Entities;
using Kubera.Data.Store.Base;
using Kubera.General.Store;

namespace Kubera.Data.Store
{
    public class GroupStore : BaseDbCrudStore<Group>, IGroupStore
    {
        public GroupStore(IApplicationDbContext applicationDbContext) 
            : base(applicationDbContext)
        {
        }
    }

    public interface IGroupStore : ICrudStore<Group>
    {
    }
}
