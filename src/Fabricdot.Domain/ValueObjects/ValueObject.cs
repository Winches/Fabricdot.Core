using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using Fabricdot.Domain.SharedKernel;

namespace Fabricdot.Domain.ValueObjects;

public abstract class ValueObject
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyCollection<PropertyInfo>> _properties = new();

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }

    public override int GetHashCode()
    {
        return GetAtomicValues().Select(x => x?.GetHashCode() ?? 0)
                                .Aggregate((x, y) => x ^ y);
    }

    public override string? ToString()
    {
        return $"{{{GetProperties().Select(v => $"{v.Name}: {v.GetValue(this)}").JoinAsString(',')}}}";
    }

    protected virtual IEnumerable<PropertyInfo> GetProperties()
    {
        return _properties.GetOrAdd(
            GetType(),
            t => t
                .GetTypeInfo()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(v => !v.IsDefined(typeof(IgnoreMemberAttribute)))
                .OrderBy(p => p.Name)
                .ToImmutableList());
    }

    protected virtual IEnumerable<object?> GetAtomicValues()
    {
        return GetProperties().Select(x => x.GetValue(this));
    }
}
