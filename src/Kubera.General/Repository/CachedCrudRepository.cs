using Kubera.General.Entities;
using Kubera.General.Services;
using Kubera.General.Store;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.General.Repository
{
    public interface ICachedCrudRepository<TEntity, TKey> : ICachedReadOnlyRepository<TEntity, TKey>, ICudRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
    }
    public interface ICachedCrudRepository<TEntity> : ICachedReadOnlyRepository<TEntity>, ICudRepository<TEntity>
        where TEntity : IEntity
    {
    }

    public class CachedCrudRepository<TEntity, TKey> : CachedReadOnlyRepository<TEntity, TKey>, ICachedCrudRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        protected new ICrudStore<TEntity, TKey> Store { get; }

        public CachedCrudRepository(ICrudStore<TEntity, TKey> store, ICacheService cacheService) 
            : base(store, cacheService)
        {
            Store = store;
        }

        public async ValueTask<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            Clear();

            return await Store.Add(entity, cancellationToken);
        }

        public async ValueTask Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            Clear();

            await Store.Update(entity, cancellationToken);
        }

        public async ValueTask Delete(TKey[] keys, bool hardDelete = false, CancellationToken cancellationToken = default)
        {
            base.Clear();

            await Store.Delete(keys, hardDelete, cancellationToken);
        }
    }

    public class CachedCrudRepository<TEntity> : CachedCrudRepository<TEntity, Guid>, ICrudRepository<TEntity>
        where TEntity : IEntity
    {
        public CachedCrudRepository(ICrudStore<TEntity> store, ICacheService cacheService)
            : base(store, cacheService)
        {
        }
    }
}
