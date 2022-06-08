using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace Fabricdot.Domain.ValueObjects;

public abstract class SingleValueObject<T> : ValueObject, ISingleValueObject, IComparable where T : IComparable
{
    private static readonly Type _type = typeof(T);

    public T Value { get; protected set; }

    protected SingleValueObject(T value)
    {
        if (value is Enum && !Enum.IsDefined(_type, value))
        {
            throw new ArgumentException($"The value '{value}' isn't defined in enum '{_type.PrettyPrint()}'");
        }
        Value = value;
    }

    public object GetValue() => Value;

    public int CompareTo(object? obj)
    {
        Guard.Against.Null(obj, nameof(obj));

        return obj is SingleValueObject<T> other
            ? Value.CompareTo(other.Value)
            : throw new ArgumentException($"Cannot compare '{_type.PrettyPrint()}' and '{obj.GetType().PrettyPrint()}'");
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}