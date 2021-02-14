using Kubera.Data.Entities;
using System.Threading.Tasks;

namespace Kubera.Business.Entities
{
    public interface IDefaultGroups
    {
        ValueTask<Group> GetCommodity();
        ValueTask<Group> GetCrypto();
        ValueTask<Group> GetStock();
    }
}