using System.Collections.Generic;

namespace Fabricdot.Domain.Entities
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        /// <summary>
        ///     identity key
        /// </summary>
        public TKey Id { get; protected set; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((Entity<TKey>)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(Id);
        }

        /// <inheritdoc />
        public override string ToString() => $"Entity:{GetType().Name}, Id:{Id}";

        protected bool Equals(Entity<TKey> other)
        {
            return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
        }
    }
}