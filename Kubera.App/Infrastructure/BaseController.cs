using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kubera.App.Infrastructure
{

    [Authorize]
    [ApiController]
    [Route("api/{v:apiVersion}/[controller]")]
    public class BaseController : ControllerBase
    {
    }
}
