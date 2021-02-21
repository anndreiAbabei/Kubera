using Kubera.General.Services;
using System;
using System.Linq;
using System.Runtime.Caching;

namespace Kubera.Business.Services
{
    public class CacheService : ICacheService
    {
        private static readonly MemoryCache _memoryCache = MemoryCache.Default;

        private readonly CacheItemPolicy _defaultCachePolicy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
        };

        public DateTimeOffset? AbsoluteExpiration
        {
            get => _defaultCachePolicy.AbsoluteExpiration != ObjectCache.InfiniteAbsoluteExpiration
                        ? _defaultCachePolicy.AbsoluteExpiration
                        : null;
            set => _defaultCachePolicy.AbsoluteExpiration = value == null
                                                                ? ObjectCache.InfiniteAbsoluteExpiration
                                                                : value.Value;
        }

        public TimeSpan? SlidingExpiration
        {
            get => _defaultCachePolicy.SlidingExpiration != ObjectCache.NoSlidingExpiration
                        ? _defaultCachePolicy.SlidingExpiration
                        : null;
            set => _defaultCachePolicy.SlidingExpiration = value == null
                                                                ? ObjectCache.NoSlidingExpiration
                                                                : value.Value;
        }

        public virtual void Add<T>(T entity, string key)
        {
            var cacheTokenKey = CreateCacheTokenKey<T>();
            AddImpl(entity, key, cacheTokenKey);
        }

        public void Add<T>(T entity, string key, string[] regions)
        {
            AddImpl(entity, key, regions);
        }

        public virtual T Get<T>(string key)
        {
            key = CreateKey<T>(key);

            if (_memoryCache.Get(key) is not CachedEntity<T> entity)
                return default;

            return entity.Entity;
        }

        public virtual void Remove<T>(string key)
        {
            key = CreateKey<T>(key);

            RemoveExpiredEntity(key);
        }

        public virtual void RemoveAll<T>()
        {
            var keysToRemve = _memoryCache.Where(kvp => kvp.Value is CachedEntity<T>)
                                          .Select(kvp => kvp.Key)
                                          .ToList();

            foreach (var key in keysToRemve)
                RemoveExpiredEntity(key);
        }

        public void RemoveRegion(string region)
        {
            var keysToRemve = _memoryCache.Where(kvp => kvp.Value is CachedEntity ce && ce.Regions.Contains(region))
                                          .Select(kvp => kvp.Key)
                                          .ToList();

            foreach (var key in keysToRemve)
                RemoveExpiredEntity(key);

        }

        public virtual void Clear()
        {
            _memoryCache.Trim(100);
        }

        protected virtual string CreateKey<T>(string key) => $"{typeof(T).FullName}[{key}]";

        private void AddImpl<T>(T entity, string key, params string[] regions)
        {
            key = CreateKey<T>(key);

            _memoryCache.Add(key, new CachedEntity<T>(entity, regions), _defaultCachePolicy);
        }

        private static string CreateCacheTokenKey<T>() => typeof(T).FullName;

        private static void RemoveExpiredEntity(string key) => _memoryCache.Remove(key, CacheEntryRemovedReason.Removed);

        private class CachedEntity
        {
            public string[] Regions { get; }

            public CachedEntity(string[] regions)
            {
                Regions = regions;
            }
        }

        private class CachedEntity<T> : CachedEntity
        {
            public T Entity { get; }

            public CachedEntity(T entity, string[] regions)
                : base(regions)
            {
                Entity = entity;
            }
        }
    }
}
