using Kubera.App.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Kubera.Application.Features.Queries.GetWallets.V1;

namespace Kubera.App.Controllers.V1
{
    [ApiVersion("1.0")]
    public class WalletController : BaseController
    {
        public WalletController(IMediator mediator)
            : base(mediator)
        {
        }

        /// <summary>
        /// Gets all user used wallets
        /// </summary>
        /// <returns>An object with a wallet collection in it (ordered by the last usage)</returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetWalletsOutput), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetWalletsOutput>> GetWallets()
        {
            var query = new GetWalletsQuery();

            return await ExecuteRequest(query).ConfigureAwait(false);
        }
    }
}
