using Kubera.Data.Entities;
using System.Threading.Tasks;

namespace Kubera.Business.Entities
{
    public interface IDefaultGroups
    {
        ValueTask<Group> Commodity();
        ValueTask<Group> Crypto();
        ValueTask<Group> Stock();
    }
}