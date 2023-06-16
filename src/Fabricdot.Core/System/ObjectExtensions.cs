using System.ComponentModel;
using System.Globalization;
using Ardalis.GuardClauses;

namespace System;

public static class ObjectExtensions
{
    /// <summary>
    ///     Indicates whether the obj is null.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsNull(this object? obj) => obj is null;

    /// <summary>
    ///     ( <typeparamref name="T" />) <paramref name="obj" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T? Cast<T>(this object? obj) => (T?)obj;

    /// <summary>
    ///     <paramref name="obj" /> as <typeparamref name="T" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T? As<T>(this object? obj) => obj is T to ? to : default;

    /// <summary>
    ///     Converts object to a value or enum type using <see
    ///     cref="Convert.ChangeType(object,TypeCode)" /> , <see cref="Enum.Parse(Type,string)"
    ///     /> or <see cref="Cast{T}(object)" />.
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? To<T>(this object value)
    {
        Guard.Against.Null(value, nameof(value));

        var type = typeof(T);
        if (type == typeof(Guid))
        {
            return (T?)TypeDescriptor.GetConverter(type)
                                    .ConvertFromInvariantString(value.ToString()!);
        }

        if (type.IsEnum)
        {
            return (T)Enum.Parse(type, value.ToString()!);
        }

        return (T)Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
    }
}
