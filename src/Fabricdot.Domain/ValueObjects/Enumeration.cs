using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;

namespace Fabricdot.Domain.ValueObjects;

/// <summary>
///     Enum object
/// </summary>
public abstract class Enumeration : IComparable
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyCollection<object>> _fields = new();

    public string Name { get; }

    public int Value { get; }

    protected Enumeration(
        int value,
        string name)
    {
        Value = value;
        Name = name;
    }

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        var values = _fields.GetOrAdd(
            typeof(T),
            t => t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                  .Select(v => v.GetValue(null))
                  .Where(v => v is not null)
                  .ToImmutableList()!);
        return values.Cast<T>();
    }

    public static bool operator ==(Enumeration? left, Enumeration? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Enumeration? left, Enumeration? right)
    {
        return !(left == right);
    }

    public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
    {
        return Math.Abs(firstValue.Value - secondValue.Value);
    }

    public static T FromValue<T>(int value) where T : Enumeration
    {
        return Parse<T, int>(value, "value", item => item.Value == value);
    }

    public static T FromName<T>(string displayName) where T : Enumeration
    {
        return Parse<T, string>(displayName, "display name", item => item.Name == displayName);
    }

    public override string ToString()
    {
        return Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
            return false;

        var typeMatches = GetType() == obj.GetType();
        var valueMatches = Value.Equals(otherValue.Value);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public virtual int CompareTo(object? obj)
    {
        if (obj is null)
            return 1;

        return obj is Enumeration other
            ? Value.CompareTo(other.Value)
            : throw new ArgumentException($"Cannot compare '{GetType().PrettyPrint()}' and '{obj.GetType().PrettyPrint()}'");
    }

    private static T Parse<T, TK>(
        TK value,
        string description,
        Func<T, bool> predicate) where T : Enumeration
    {
        return GetAll<T>().FirstOrDefault(predicate)
            ?? throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T).PrettyPrint()}");
    }
}