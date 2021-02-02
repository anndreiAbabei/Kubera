using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kubera.App.Infrastructure
{

    [Authorize]
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class BaseController : ControllerBase
    {
    }
}
