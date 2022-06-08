using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Events;

public class EntityCreatedEvent<TEntity> : EntityEventBase<TEntity> where TEntity : IHasDomainEvents
{
    public EntityCreatedEvent(TEntity entity) : base(entity)
    {
    }
}