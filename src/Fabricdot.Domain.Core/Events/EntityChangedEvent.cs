using Fabricdot.Domain.Core.Entities;

namespace Fabricdot.Domain.Core.Events
{
    public class EntityChangedEvent<TEntity> : EntityEventBase<TEntity> where TEntity : IEntity<object>
    {
        public EntityChangedEvent(TEntity entity) : base(entity)
        {
        }
    }
}