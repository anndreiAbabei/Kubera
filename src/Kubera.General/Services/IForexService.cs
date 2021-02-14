using System.Threading;
using System.Threading.Tasks;
using Kubera.General.Models;

namespace Kubera.General.Services
{
    public interface IForexService
    {
        ValueTask<IForexServiceResponse> GetPriceOf(string fromCode, string toCode, CancellationToken cancellationToken = default);
    }
}
