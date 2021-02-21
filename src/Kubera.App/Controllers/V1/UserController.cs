using Kubera.App.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Kubera.Application.Common.Models;
using MediatR;
using Kubera.Application.Features.Queries.GetUserInfo.V1;
using Kubera.App.Infrastructure.Extensions;

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
    }
}
