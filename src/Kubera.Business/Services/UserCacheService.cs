using Kubera.General.Services;
using Microsoft.Extensions.Logging;

namespace Kubera.Business.Services
{
    public class UserCacheService : CacheService, IUserCacheService
    {
        private readonly IUserIdAccesor _userIdAccesor;

        public string UserId => _userIdAccesor.Id;

        public UserCacheService(IUserIdAccesor userIdAccesor, ILogger<UserCacheService> logger)
            : base(logger)
        {
            _userIdAccesor = userIdAccesor;
        }

        protected override string CreateKey<T>(string key) => $"{base.CreateKey<T>(key)}.<{_userIdAccesor.Id}>";
    }
}
