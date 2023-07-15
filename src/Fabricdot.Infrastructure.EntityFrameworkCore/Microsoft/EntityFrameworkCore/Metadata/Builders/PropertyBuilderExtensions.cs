using Fabricdot.Domain.ValueObjects;
using Fabricdot.Infrastructure.EntityFrameworkCore.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class PropertyBuilderExtensions
{
    /// <summary>
    ///     Configures the property that the property is a typed key
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static PropertyBuilder<T> IsTypedKey<T>(this PropertyBuilder<T> builder)
    {
        ((PropertyBuilder)builder).IsTypedKey();
        return builder;
    }

    /// <summary>
    ///     Configures the property that the property is a typed key
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static PropertyBuilder IsTypedKey(this PropertyBuilder builder)
    {
        var clrType = builder.Metadata.ClrType;
        if (!clrType.IsAssignableToGenericType(typeof(IIdentity<>)))
        {
            throw new InvalidOperationException($"The '{clrType.PrettyPrint()}' is not a identity type.");
        }

        var type = clrType.GetInterfaces().First(v => v.IsAssignableToGenericType(typeof(IIdentity<>)));
        builder.HasConversion(typeof(SingleValueObjectConverter<,>).MakeGenericType(clrType, type.GenericTypeArguments[0]));

        return builder;
    }

    /// <summary>
    ///     Configures the property that the property is an enumeration type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static PropertyBuilder<T> IsEnumeration<T>(this PropertyBuilder<T> builder)
    {
        ((PropertyBuilder)builder).IsEnumeration();
        return builder;
    }

    /// <summary>
    ///     Configures the property that the property is an enumeration type
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static PropertyBuilder IsEnumeration(this PropertyBuilder builder)
    {
        var clrType = builder.Metadata.ClrType;
        if (!clrType.IsAssignableTo(typeof(Enumeration)))
        {
            throw new InvalidOperationException($"The '{clrType.PrettyPrint()}' is not a enumeration type.");
        }

        builder.HasConversion(typeof(EnumerationConverter<>).MakeGenericType(clrType));

        return builder;
    }
}
