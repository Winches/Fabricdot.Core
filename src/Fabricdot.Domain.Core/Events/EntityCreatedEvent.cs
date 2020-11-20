using Fabricdot.Domain.Core.Entities;

namespace Fabricdot.Domain.Core.Events
{
    public class EntityCreatedEvent<TEntity> : EntityEventBase<TEntity> where TEntity : IEntity<object>
    {
        public EntityCreatedEvent(TEntity entity) : base(entity)
        {
        }
    }
}