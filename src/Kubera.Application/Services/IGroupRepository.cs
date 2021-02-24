using Kubera.Data.Entities;
using Kubera.General.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Services
{
    public interface IGroupRepository : ICrudRepository<Group>
    {
        ValueTask<Group> GetByCode(string code, CancellationToken cancellationToken = default);
        ValueTask<bool> Exists(Group group, CancellationToken cancellationToken = default);
    }
}
