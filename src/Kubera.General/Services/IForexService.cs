using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Kubera.General.Models;

namespace Kubera.General.Services
{
    public interface IForexService
    {
        ValueTask<IResult<IForexServiceResponse>> GetPriceOf(string fromCode, string toCode, CancellationToken cancellationToken = default);
    }
}
