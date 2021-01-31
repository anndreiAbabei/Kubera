using System.Threading;
using System.Threading.Tasks;

namespace Kubera.General
{
    public interface ISeeder
    {
        ValueTask Seed(CancellationToken cancellationToken = default);
    }
}
