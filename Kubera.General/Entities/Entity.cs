using System;

namespace Kubera.General.Entities
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
    public interface IEntity : IEntity<Guid>
    {
    }

    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public virtual TKey Id { get; set; }
    }

    public abstract class Entity : Entity<Guid>, IEntity
    {
    }
}
