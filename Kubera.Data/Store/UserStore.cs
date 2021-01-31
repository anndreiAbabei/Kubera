using Kubera.Data.Entities;
using Kubera.Data.Store.Base;
using Kubera.General.Store;

namespace Kubera.Data.Store
{
    public class UserStore : BaseDbReadOnlyStore<ApplicationUser, string>, IUserStore
    {
        public UserStore(IApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }
    }

    public interface IUserStore : IStore<ApplicationUser, string>
    {
    }
}
