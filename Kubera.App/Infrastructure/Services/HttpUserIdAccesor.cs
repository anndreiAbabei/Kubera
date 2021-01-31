using Kubera.General.Services;
using Microsoft.AspNetCore.Http;

namespace Kubera.App.Infrastructure.Services
{
    public sealed class HttpUserIdAccesor : IUserIdAccesor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string Id => _httpContextAccessor.HttpContext.User.Identity.Name;

        public HttpUserIdAccesor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
