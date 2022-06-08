using System;

namespace Fabricdot.Domain.ValueObjects;

// TODO: refactor
public abstract class Identity<T> : SingleValueObject<T>, IIdentity<T> where T : IComparable
{
    /// <inheritdoc />
    protected Identity(T value) : base(value)
    {
    }
}