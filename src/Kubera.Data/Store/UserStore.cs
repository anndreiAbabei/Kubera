using Kubera.Data.Entities;
using Kubera.Data.Store.Base;
using Kubera.General.Store;
using Microsoft.Extensions.Logging;

namespace Kubera.Data.Store
{
    public class UserStore : BaseDbReadOnlyStore<ApplicationUser, string>, IUserStore
    {
        public UserStore(IApplicationDbContext applicationDbContext, ILogger<UserStore> logger)
            : base(applicationDbContext, logger)
        {
        }
    }

    public interface IUserStore : IStore<ApplicationUser, string>
    {
    }
}
