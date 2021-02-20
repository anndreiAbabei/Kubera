using System.Threading;
using System.Threading.Tasks;
using Kubera.Data.Entities;

namespace Kubera.Business.Entities.Defaults
{
    public interface IDefaultGroups
    {
        ValueTask<Group> GetCommodity(CancellationToken canellationToken = default);
        ValueTask<Group> GetCrypto(CancellationToken canellationToken = default);
        ValueTask<Group> GetStock(CancellationToken canellationToken = default);
    }
}