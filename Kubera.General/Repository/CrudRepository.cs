using Kubera.General.Entities;
using Kubera.General.Store;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.General.Repository
{

    public interface ICrudRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>, ICudRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
    }

    public interface ICrudRepository<TEntity> : ICrudRepository<TEntity, Guid>, ICudRepository<TEntity>
        where TEntity : IEntity
    {

    }

    public class CrudRepository<TEntity, TKey> : ReadOnlyRepository<TEntity, TKey>, ICrudRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        protected new ICrudStore<TEntity, TKey> Store { get; }

        public CrudRepository(ICrudStore<TEntity, TKey> store)
            : base(store)
        {
            Store = store;
        }

        public virtual ValueTask<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default) => Store.Add(entity, cancellationToken);

        public virtual ValueTask Update(TEntity entity, CancellationToken cancellationToken = default) => Store.Update(entity, cancellationToken);

        public virtual ValueTask Delete(TKey[] keys, bool hardDelete = false, CancellationToken cancellationToken = default) => Store.Delete(keys, hardDelete, cancellationToken);
    }

    public class CrudRepository<TEntity> : CrudRepository<TEntity, Guid>, ICrudRepository<TEntity>
        where TEntity : IEntity
    {
        public CrudRepository(ICrudStore<TEntity, Guid> store) 
            : base(store)
        {
        }
    }
}
