using Kubera.General.Entities;
using Kubera.General.Extensions;
using Kubera.General.Models;
using Kubera.General.Services;
using Kubera.General.Store;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.General.Repository
{
    public interface ICachedReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        void Remove(TKey[] keys);
        void Clear();
    }

    public interface ICachedReadOnlyRepository<TEntity> : ICachedReadOnlyRepository<TEntity, Guid>
        where TEntity : IEntity
    {
    }

    public class CachedReadOnlyRepository<TEntity, TKey> : ReadOnlyRepository<TEntity, TKey>, ICachedReadOnlyRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        protected virtual ICacheOptions Options { get; }

        protected virtual ICacheService CacheService { get; }

        public CachedReadOnlyRepository(IStore<TEntity, TKey> store,
            ICacheService cacheService,
            ICacheOptions options)
            : base(store)
        {
            CacheService = cacheService;
            Options = options;
        }

        public virtual void Remove(TKey[] keys)
        {
            if (!Options.UseCache)
                CacheService.Remove<TEntity>(keys.ToKey());
        }



        public virtual void Clear()
        {
            if (Options.UseCache)
                CacheService.Clear();
        }



        public override async ValueTask<TEntity> GetById(TKey[] keys, CancellationToken cancellationToken = default)
        {
            if (!Options.UseCache)
                return await base.GetById(keys, cancellationToken).ConfigureAwait(false);

            return await CacheService.GetOrAdd(keys.ToKey(),
                                               async () => await base.GetById(keys, cancellationToken)
                                                                     .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public override IQueryable<TEntity> GetAll()
        {
            if (!Options.UseCache)
                return base.GetAll();

            const string keys = "All";

            return CacheService.GetOrAdd(keys, () => base.GetAll());
        }

        protected virtual object[] CalculateGetAllKey(IPaging paging, IFilter dateFilter)
        {
            return new object[] { $"{paging?.Items}_{paging?.Page}|{dateFilter?.From?.ToString("o")}_{dateFilter?.To?.ToString("o")}" };
        }
    }

    public class CachedReadOnlyRepository<TEntity> : CachedReadOnlyRepository<TEntity, Guid>, ICachedReadOnlyRepository<TEntity>
        where TEntity : IEntity
    {
        public CachedReadOnlyRepository(IStore<TEntity> store,
            ICacheService cacheService,
            ICacheOptions options) 
            : base(store, cacheService, options)
        {
        }
    }
}
