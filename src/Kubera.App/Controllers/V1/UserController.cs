using Kubera.App.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Kubera.Application.Common.Models;
using MediatR;
using Kubera.Application.Features.Queries.GetUserInfo.V1;
using Kubera.Application.Features.Commands.UpdateUserCurrency.V1;

namespace Kubera.App.Controllers.V1
{
    [ApiVersion("1.0")]
    public class UserController : BaseController
    {
        public UserController(IMediator mediator)
            : base(mediator)
        {
        }

        /// <summary>
        /// Gets user info
        /// </summary>
        /// <returns>User information object</returns>
        [HttpGet]
        [ProducesResponseType(typeof(UserInfoModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserInfoModel>> GetUserInfo()
        {
            var query = new GetUserInfoQuery();

            return await ExecuteRequest(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Update user prefferd currency
        /// </summary>
        /// <returns>User information object</returns>
        [HttpPatch("currency")]
        [ProducesResponseType(typeof(UserInfoModel), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateUserCurrency([FromBody] UpdateUserCurrencyModel model)
        {
            var command = new UpdateUserCurrencyCommand();

            return await ExecuteRequest(command).ConfigureAwait(false);
        }
    }
}
