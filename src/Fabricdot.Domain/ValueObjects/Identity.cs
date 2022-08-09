using System;
using Ardalis.GuardClauses;

namespace Fabricdot.Domain.ValueObjects;

public abstract class Identity<T> : SingleValueObject<T>, IIdentity<T> where T : IComparable
{
    /// <inheritdoc />
    protected Identity(T value) : base(value)
    {
        if (value is string str)
            Guard.Against.NullOrWhiteSpace(str, nameof(value));
        else if (value is Guid guid)
            Guard.Against.NullOrEmpty(guid, nameof(value));
        else
            Guard.Against.Null(value, nameof(value));
    }
}

public abstract class Identity : Identity<Guid>
{
    protected Identity(Guid value) : base(value)
    {
    }
}