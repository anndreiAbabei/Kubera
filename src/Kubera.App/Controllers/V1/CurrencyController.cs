using Kubera.App.Infrastructure;
using Kubera.Application.Common.Models;
using Kubera.Application.Features.Queries.GetCurrencies.V1;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kubera.App.Controllers.V1
{
    [ApiVersion("1.0")]
    public class CurrencyController : BaseController
    {
        public CurrencyController(IMediator mediator)
            : base(mediator)
        {
        }

        /// <summary>
        /// Get all currencies suported by the platform
        /// </summary>
        /// <returns>Collection of currencies</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CurrencyModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CurrencyModel>>> GetCurrencies()
        {
            var query = new GetCurrenciesQuery();

            return await ExecuteRequest(query).ConfigureAwait(false);
        }
    }
}
