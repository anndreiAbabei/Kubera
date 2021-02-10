using Kubera.General.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.General.Store
{
    public interface ISingleEntityStore<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        ValueTask<TEntity> GetById(TKey[] keys, CancellationToken cancellationToken = default);
    }

    public interface ISingleEntityStore<TEntity> : ISingleEntityStore<TEntity, Guid>
       where TEntity : IEntity
    {
    }

    public interface IStore<TEntity, TKey> : ISingleEntityStore<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        IQueryable<TEntity> GetAll();
    }

    public interface IStore<TEntity> : IStore<TEntity, Guid>, ISingleEntityStore<TEntity>
        where TEntity : IEntity
    {
    }

    public interface ICrudStore<TEntity, TKey> : IStore<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        ValueTask<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default);

        ValueTask Update(TEntity entity, CancellationToken cancellationToken = default);

        ValueTask Delete(TKey[] keys, bool hardDelete = false, CancellationToken cancellationToken = default);
    }

    public interface ICrudStore<TEntity> : ICrudStore<TEntity, Guid>
        where TEntity : IEntity
    {
    }
}
