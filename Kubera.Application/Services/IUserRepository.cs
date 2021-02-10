using Kubera.Data.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Services
{
    public interface IUserRepository
    {
        ValueTask<ApplicationUser> GetMe(CancellationToken cancellationToken = default);
    }
}
