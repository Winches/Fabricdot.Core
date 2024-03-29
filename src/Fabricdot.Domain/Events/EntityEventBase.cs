using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Events;

public abstract class EntityEventBase<TEntity> : DomainEventBase where TEntity : IHasDomainEvents
{
    public TEntity Entity { get; }

    protected EntityEventBase(TEntity entity)
    {
        Entity = entity;
    }
}
