using Kubera.General.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Kubera.Business.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheChangeToken _baseExpirationToken;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _baseExpirationToken = new CacheChangeToken();
        }

        public virtual void Add<T>(T entity, params object[] keys)
        {
            var key = CreateKey<T>(keys);
            var cacheTokenKey = CreateCacheTokenKey<T>();
            var token = _memoryCache.TryGetValue<IChangeToken>(cacheTokenKey, out var tk)
                        ? tk
                        : new CacheChangeToken(_baseExpirationToken);

            _memoryCache.Set(key, entity, token);
        }

        public virtual T Get<T>(params object[] keys)
        {
            var key = CreateKey<T>(keys);

            return _memoryCache.TryGetValue<T>(key, out var entity)
                    ? entity
                    : default;
        }

        public virtual void Remove<T>(params object[] keys)
        {
            var key = CreateKey<T>(keys);

            _memoryCache.Remove(key);
        }

        public virtual void RemoveAll<T>()
        {
            var cacheTokenKey = CreateCacheTokenKey<T>();

            if (_memoryCache.TryGetValue<CacheChangeToken>(cacheTokenKey, out var tk))
                tk.HasChanged = true;
        }

        public virtual void Clear()
        {
            _baseExpirationToken.HasChanged = true;
        }

        protected virtual string CreateKey<T>(params object[] keys)
        {
            return $"{typeof(T).FullName}[{string.Join(".", keys)}]";
        }

        private static string CreateCacheTokenKey<T>()
        {
            return $"CACHE_TOKEN.{typeof(T).FullName}";
        }
    }
}
