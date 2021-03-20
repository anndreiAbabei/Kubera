using Kubera.General.Services;
using System;
using System.Linq;
using System.Runtime.Caching;
using Microsoft.Extensions.Logging;
using Kubera.General.Extensions;

namespace Kubera.Business.Services
{
    public class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> _logger;
        private static readonly MemoryCache _memoryCache = MemoryCache.Default;

        private readonly CacheItemPolicy _defaultCachePolicy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
        };



        public CacheService(ILogger<CacheService> logger)
        {
            _logger = logger;
        }

        public DateTimeOffset? AbsoluteExpiration
        {
            get => _defaultCachePolicy.AbsoluteExpiration != ObjectCache.InfiniteAbsoluteExpiration
                        ? _defaultCachePolicy.AbsoluteExpiration
                        : null;
            set => _defaultCachePolicy.AbsoluteExpiration = value ?? ObjectCache.InfiniteAbsoluteExpiration;
        }

        public TimeSpan? SlidingExpiration
        {
            get => _defaultCachePolicy.SlidingExpiration != ObjectCache.NoSlidingExpiration
                        ? _defaultCachePolicy.SlidingExpiration
                        : null;
            set => _defaultCachePolicy.SlidingExpiration = value ?? ObjectCache.NoSlidingExpiration;
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
            var evId = new EventId(Guid.NewGuid().GetHashCode(), nameof(Get));
            key = CreateKey<T>(key);

            _logger.LogTrace(evId, $"Try get [{key}] from cache of type [{typeof(T).FullName}]");

            if (_memoryCache.Get(key) is not CachedEntity<T> entity)
            {
                _logger.LogTrace(evId, $"Key [{key}] not found in cache, returns default");
                return default;
            }
            
            _logger.LogTrace(evId, $"Key [{key}] had been found in cache having regions [{entity.Regions.Join()}]");
            return entity.Entity;
        }

        public virtual void Remove<T>(string key)
        {
            key = CreateKey<T>(key);
            _logger.LogTrace($"Removing [{key}] from cache of type [{typeof(T).FullName}]");

            RemoveExpiredEntity(key);
        }

        public virtual void RemoveAll<T>()
        {
            _logger.LogTrace($"Removing all [{typeof(T).FullName}] from cache");
            var keysToRemve = _memoryCache.Where(kvp => kvp.Value is CachedEntity<T>)
                                          .Select(kvp => kvp.Key)
                                          .ToList();

            foreach (var key in keysToRemve)
                RemoveExpiredEntity(key);
        }

        public void RemoveRegion(string region)
        {
            _logger.LogTrace($"Removing region [{region}] from cache");
            var keysToRemve = _memoryCache.Where(kvp => kvp.Value is CachedEntity ce && ce.Regions.Contains(region))
                                          .Select(kvp => kvp.Key)
                                          .ToList();

            foreach (var key in keysToRemve)
                RemoveExpiredEntity(key);

        }

        public virtual void Clear()
        {
            _logger.LogTrace("Clear cache");
            _memoryCache.Trim(100);
        }

        protected virtual string CreateKey<T>(string key) => $"{typeof(T).FullName}[{key}]";

        private void AddImpl<T>(T entity, string key, params string[] regions)
        {
            key = CreateKey<T>(key);
            
            _logger.LogTrace($"Adding [{key}] to cache, at regions [{regions.Join()}], of type [{typeof(T).FullName}]");
            _memoryCache.Add(key, new CachedEntity<T>(entity, regions), _defaultCachePolicy);
        }

        private static string CreateCacheTokenKey<T>() => typeof(T).FullName;

        private static void RemoveExpiredEntity(string key) => _memoryCache.Remove(key, CacheEntryRemovedReason.Removed);

        private class CachedEntity
        {
            public string[] Regions { get; }



            protected CachedEntity(string[] regions)
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
