using Kubera.App.Infrastructure;
using Kubera.App.Models;
using Kubera.Business.Repository;
using Kubera.General.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Kubera.Business.Entities;

namespace Kubera.App.Controllers.V1
{
    [ApiVersion("1.0")]
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserIdAccesor _userIdAccesor;

        public UserController(IUserRepository userRepository, IUserIdAccesor userIdAccesor)
        {
            _userRepository = userRepository;
            _userIdAccesor = userIdAccesor;
        }

        /// <summary>
        /// Gets user info
        /// </summary>
        /// <returns>User information object</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CurrencyModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserInfoModel>> GetUserInfo()
        {
            var user = await _userRepository.GetMe(HttpContext.RequestAborted)
                .ConfigureAwait(false);
            var result = new UserInfoModel
            {
                Email = user.Email,
                FullName = user.FullName,
                Settings = JsonSerializer.Deserialize<UserSettings>(user.Settings)
            };

            return Ok(result);
        }
    }
}
