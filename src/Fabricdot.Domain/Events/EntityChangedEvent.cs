using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Events
{
    public class EntityChangedEvent<TEntity> : EntityEventBase<TEntity> where TEntity : IHasDomainEvents
    {
        public EntityChangedEvent(TEntity entity) : base(entity)
        {
        }
    }
}