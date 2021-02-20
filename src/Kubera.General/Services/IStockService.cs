using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Kubera.General.Models;

namespace Kubera.General.Services
{
    public interface IStockService
    {
        ValueTask<IResult<IStockCompanyResponse>> GetCompany(string company, CancellationToken cancellationToken = default);
        ValueTask<IResult<IStockServiceResponse>> GetPriceOf(string stock, string toCode, CancellationToken cancellationToken = default);
    }
}
