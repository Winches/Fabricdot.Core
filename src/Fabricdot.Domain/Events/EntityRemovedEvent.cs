using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Events
{
    public class EntityRemovedEvent<TEntity> : EntityEventBase<TEntity> where TEntity : IHasDomainEvents
    {
        public EntityRemovedEvent(TEntity entity) : base(entity)
        {
        }
    }
}