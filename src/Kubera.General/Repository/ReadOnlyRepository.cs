using Kubera.General.Entities;
using Kubera.General.Store;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.General.Repository
{
    public interface IReadOnlyRepository<TEntity, TKey> : IStore<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
    }

    public interface IReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity, Guid>
        where TEntity : IEntity
    {
    }

    public abstract class ReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        protected virtual IStore<TEntity, TKey> Store { get; }

        protected ReadOnlyRepository(IStore<TEntity, TKey> store)
        {
            Store = store;
        }

        public virtual ValueTask<TEntity> GetById(TKey[] keys, CancellationToken cancellationToken = default) => Store.GetById(keys, cancellationToken);
        
        public virtual IQueryable<TEntity> GetAll() => Store.GetAll();
    }

    public abstract class ReadOnlyRepository<TEntity> : ReadOnlyRepository<TEntity, Guid>, IReadOnlyRepository<TEntity>
        where TEntity : IEntity
    {
        protected ReadOnlyRepository(IStore<TEntity> store) 
            : base(store)
        {
        }
    }
}
