using Fabricdot.Domain.Internal;

namespace Fabricdot.Domain.Entities;

public abstract class Entity<TKey> : IEntity<TKey> where TKey : notnull
{
    /// <summary>
    ///     identity key
    /// </summary>
    public TKey Id { get; protected set; } = default!;

    protected Entity()
    {
        EntityInitializer.Instance.Initialize(this);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is not null
            && (ReferenceEquals(this, obj) || (obj.GetType() == GetType() && Equals((Entity<TKey>)obj)));
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