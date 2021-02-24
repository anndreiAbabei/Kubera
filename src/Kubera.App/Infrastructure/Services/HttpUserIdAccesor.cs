using Kubera.General.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Kubera.App.Infrastructure.Services
{
    public sealed class HttpUserIdAccesor : IUserIdAccesor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string Id => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public HttpUserIdAccesor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
