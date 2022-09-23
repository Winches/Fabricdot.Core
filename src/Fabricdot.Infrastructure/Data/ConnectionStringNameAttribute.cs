using System.Reflection;
using Ardalis.GuardClauses;

namespace Fabricdot.Infrastructure.Data;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class ConnectionStringNameAttribute : Attribute
{
    public string Name { get; }

    public ConnectionStringNameAttribute(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));

        Name = name;
    }

    public static string GetConnStringName<T>()
    {
        return GetConnStringName(typeof(T));
    }

    public static string GetConnStringName(Type type)
    {
        var nameAttribute = type.GetTypeInfo().GetCustomAttribute<ConnectionStringNameAttribute>();
        return nameAttribute == null ? (type.FullName ?? type.Name) : nameAttribute.Name;
    }
}