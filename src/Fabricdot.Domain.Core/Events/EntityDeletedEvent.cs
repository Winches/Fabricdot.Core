using Fabricdot.Domain.Core.Entities;

namespace Fabricdot.Domain.Core.Events
{
    public class EntityDeletedEvent<TEntity> : EntityEventBase<TEntity> where TEntity : IHasDomainEvents
    {
        public EntityDeletedEvent(TEntity entity) : base(entity)
        {
        }
    }
}