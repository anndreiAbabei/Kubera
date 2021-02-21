using Kubera.General.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Kubera.Business.Services
{
    public class UserCacheService : CacheService, IUserCacheService
    {
        private readonly IUserIdAccesor _userIdAccesor;

        public string UserId => _userIdAccesor.Id;

        public UserCacheService(IMemoryCache memoryCache, IUserIdAccesor userIdAccesor)
            : base(memoryCache)
        {
            _userIdAccesor = userIdAccesor;
        }

        protected override string CreateKey<T>(params object[] keys) => $"{base.CreateKey<T>(keys)}.<{_userIdAccesor.Id}>";
    }
}
