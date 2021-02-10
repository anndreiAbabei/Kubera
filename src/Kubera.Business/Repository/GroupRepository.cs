using Kubera.Data.Entities;
using Kubera.Data.Store;
using Kubera.General.Models;
using Kubera.General.Repository;
using Kubera.General.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Business.Repository
{
    public class GroupRepository : CachedCrudRepository<Group>, IGroupRepository
    {
        private readonly IUserIdAccesor _userIdAccesor;

        public GroupRepository(IGroupStore store, ICacheService cacheService, IUserIdAccesor userIdAccesor) 
            : base(store, cacheService)
        {
            _userIdAccesor = userIdAccesor;
        }

        public async ValueTask<Group> GetByCode(string code, CancellationToken cancellationToken = default)
        {
            var query = await GetAll(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return await query
                .FirstOrDefaultAsync(g => g.Code == code)
                .ConfigureAwait(false);
        }

        public async ValueTask<bool> Exists(Group group, CancellationToken cancellationToken = default)
        {
            var query = await GetAll(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return await query
                .AnyAsync(g => g.Id != group.Id &&
                               (g.Code == group.Code || g.Name == group.Name), 
                         cancellationToken)
                .ConfigureAwait(false);
        }

        public override async ValueTask<IQueryable<Group>> GetAll(IPaging paging = null, IDateFilter dateFilter = null, CancellationToken cancellationToken = default)
        {
            var user = _userIdAccesor.Id;
            var query = await base.GetAll(paging, dateFilter, cancellationToken)
                .ConfigureAwait(false);

            return query.Where(a => a.OwnerId == user);
        }
    }

    public interface IGroupRepository : ICrudRepository<Group>
    {
        ValueTask<Group> GetByCode(string code, CancellationToken cancellationToken = default);
        ValueTask<bool> Exists(Group group, CancellationToken cancellationToken = default);
    }
}
