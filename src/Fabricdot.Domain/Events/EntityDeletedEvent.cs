using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Events
{
    public class EntityDeletedEvent<TEntity> : EntityEventBase<TEntity> where TEntity : IHasDomainEvents
    {
        public EntityDeletedEvent(TEntity entity) : base(entity)
        {
        }
    }
}