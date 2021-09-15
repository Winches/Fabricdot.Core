using Fabricdot.Domain.Core.Entities;

namespace Fabricdot.Domain.Core.Events
{
    public abstract class EntityEventBase<TEntity> : DomainEventBase where TEntity : IHasDomainEvents
    {
        public TEntity Entity { get; }

        public EntityEventBase(TEntity entity)
        {
            Entity = entity;
        }
    }
}