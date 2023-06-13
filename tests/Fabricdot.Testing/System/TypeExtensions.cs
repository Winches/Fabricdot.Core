using System.Reflection;

namespace System;

internal static class TypeExtensions
{
    public static MethodInfo? GetEqualsMethod(this Type type)
    {
        return type.GetMethod(nameof(object.Equals), new[] { typeof(object) });
    }

    public static MethodInfo? GetEqualityOperatorMethod(this Type type)
    {
        return type.GetMethod("op_Equality", new[] { type, type });
    }

    public static MethodInfo? GetInequalityOperatorMethod(this Type type)
    {
        return type.GetMethod("op_Inequality", new[] { type, type });
    }

    public static MethodInfo? GetStronglyTypedEqualsMethod(this Type type)
    {
        return type.GetMethod(nameof(object.Equals), new[] { type });
    }
}
