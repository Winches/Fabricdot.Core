using System.Collections.Generic;

namespace Fabricdot.Domain.Core.Entities
{
    public abstract class EntityBase<TKey> : IEntity<TKey>
    {
        /// <summary>
        ///     identity key
        /// </summary>
        public TKey Id { get; protected set; }

        protected bool Equals(EntityBase<TKey> other)
        {
            return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((EntityBase<TKey>)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(Id);
        }

        /// <inheritdoc />
        public override string ToString() => $"Entity:{GetType().Name}, Id:{Id}";
    }
}