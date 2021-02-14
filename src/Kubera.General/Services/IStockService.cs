using System.Threading;
using System.Threading.Tasks;
using Kubera.General.Models;

namespace Kubera.General.Services
{
    public interface IStockService
    {
        ValueTask<IStockCompanyResponse> GetCompany(string company, CancellationToken cancellationToken = default);
        ValueTask<IStockServiceResponse> GetPriceOf(string stock, string toCode, CancellationToken cancellationToken = default);
    }
}
