using Kubera.Application.Services;
using Kubera.Data.Entities;
using Kubera.Data.Store;
using Kubera.General.Repository;
using Kubera.General.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Business.Repository
{
    public class UserRepository : CachedCrudRepository<ApplicationUser, string>, IUserRepository
    {
        private readonly IUserIdAccesor _userIdAccesor;

        public UserRepository(IUserStore store,
            ICacheService cacheService,
            IUserIdAccesor userIdAccesor,
            ICacheOptions options)
            : base(store, cacheService, options)
        {
            _userIdAccesor = userIdAccesor;
        }

        public async ValueTask<ApplicationUser> GetMe(CancellationToken cancellationToken = default)
        {
            return await GetById(new[] { _userIdAccesor.Id }, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
