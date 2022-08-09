using System;
using System.Collections.Generic;

namespace Fabricdot.Domain.ValueObjects;

public abstract class SingleValueObject<T> : ValueObject, ISingleValueObject, IComparable where T : IComparable
{
    private static readonly Type _type = typeof(T);

    public virtual T Value { get; protected set; }

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
        if (obj is null)
            return 1;

        return obj is SingleValueObject<T> other
            ? Value.CompareTo(other.Value)
            : throw new ArgumentException($"Cannot compare '{GetType().PrettyPrint()}' and '{obj.GetType().PrettyPrint()}'");
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public override string? ToString() => Value.ToString();
}