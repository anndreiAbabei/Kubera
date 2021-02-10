using Kubera.General.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.General.Repository
{
    public interface IAddeable<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        ValueTask<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default);
    }

    public interface IAddeable<TEntity> : IAddeable<TEntity, Guid>
        where TEntity : IEntity
    {
    }

    public interface IUpdateable<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        ValueTask Update(TEntity entity, CancellationToken cancellationToken = default);
    }

    public interface IUpdateable<TEntity> : IUpdateable<TEntity, Guid>
        where TEntity : IEntity
    {
    }

    public interface IDeleateable<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        ValueTask Delete(TKey[] keys, bool hardDelete = false, CancellationToken cancellationToken = default);
    }
    public interface IDeleateable<TEntity> : IDeleateable<TEntity, Guid>
        where TEntity : IEntity
    {
    }

    public interface ICudRepository<TEntity, TKey> : IAddeable<TEntity, TKey>, IUpdateable<TEntity, TKey>, IDeleateable<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {

    }

    public interface ICudRepository<TEntity> : ICudRepository<TEntity, Guid>
        where TEntity : IEntity
    {

    }
}
