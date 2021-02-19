using System.Threading.Tasks;
using Kubera.Data.Entities;

namespace Kubera.Business.Entities.Defaults
{
    public interface IDefaultGroups
    {
        ValueTask<Group> GetCommodity();
        ValueTask<Group> GetCrypto();
        ValueTask<Group> GetStock();
    }
}