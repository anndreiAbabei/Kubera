using Kubera.Data.Entities;
using Kubera.Data.Store;
using Kubera.General.Repository;
using Kubera.General.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Business.Repository
{
    public class GroupRepository : CrudRepository<Group>, IGroupRepository
    {
        private readonly IUserIdAccesor _userIdAccesor;

        public GroupRepository(IGroupStore store, IUserIdAccesor userIdAccesor) 
            : base(store)
        {
            _userIdAccesor = userIdAccesor;
        }

        public async ValueTask<Group> GetByCode(string code, CancellationToken cancellationToken = default)
        {
            return await GetAll()
                .FirstOrDefaultAsync(g => g.Code == code)
                .ConfigureAwait(false);
        }

        public async ValueTask<bool> Exists(Group group, CancellationToken cancellationToken = default)
        {
            return await GetAll()
                .AnyAsync(g => g.Id != group.Id &&
                               (g.Code == group.Code || g.Name == group.Name), 
                         cancellationToken)
                .ConfigureAwait(false);
        }

        public override IQueryable<Group> GetAll()
        {
            var user = _userIdAccesor.Id;

            return base.GetAll().Where(a => a.OwnerId == null || a.OwnerId == user);
        }
    }

    public interface IGroupRepository : ICrudRepository<Group>
    {
        ValueTask<Group> GetByCode(string code, CancellationToken cancellationToken = default);
        ValueTask<bool> Exists(Group group, CancellationToken cancellationToken = default);
    }
}
