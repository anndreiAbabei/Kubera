using Kubera.General.Services;
using System;
using System.Threading.Tasks;

namespace Kubera.General.Extensions
{
    public static class CacheServiceEx
    {
        public static T Get<T>(this ICacheService cacheService, object key)
        {
            if (cacheService == null)
                throw new ArgumentNullException(nameof(cacheService));

            return cacheService.Get<T>(key);
        }

        public static void Add<T>(this ICacheService cacheService, object key, T entity)
        {
            if (cacheService == null)
                throw new ArgumentNullException(nameof(cacheService));

            cacheService.Add(entity, key);
        }

        public static void Remove<T>(this ICacheService cacheService, object key)
        {
            if (cacheService == null)
                throw new ArgumentNullException(nameof(cacheService));

            cacheService.Remove<T>(key);
        }


        public static T GetOrAdd<T>(this ICacheService cacheService, object key, Func<T> entityHandler)
            where T : class
        {
            return GetOrAdd(cacheService, new[] { key }, entityHandler);
        }

        public static async ValueTask<T> GetOrAdd<T>(this ICacheService cacheService, object key, Func<ValueTask<T>> entityHandler)
            where T : class
        {
            return await GetOrAdd(cacheService, new[] { key }, entityHandler)
                .ConfigureAwait(false);
        }

        public static T GetOrAdd<T>(this ICacheService cacheService, object[] keys, Func<T> entityHandler)
            where T : class
        {
            if (cacheService == null)
                throw new ArgumentNullException(nameof(cacheService));

            if (entityHandler == null)
                throw new ArgumentNullException(nameof(entityHandler));

            var entity = cacheService.Get<T>(keys);

            if (entity != null)
                return entity;

            entity = entityHandler();

            cacheService.Add(entity, keys);

            return entity;
        }

        public static async ValueTask<T> GetOrAdd<T>(this ICacheService cacheService, object[] keys, Func<ValueTask<T>> entityHandler)
            where T : class
        {
            if (cacheService == null)
                throw new ArgumentNullException(nameof(cacheService));

            if (entityHandler == null)
                throw new ArgumentNullException(nameof(entityHandler));

            var entity = cacheService.Get<T>(keys);

            if (entity != null)
                return entity;

            entity = await entityHandler()
                .ConfigureAwait(false);

            cacheService.Add(entity, keys);

            return entity;
        }
    }
}
