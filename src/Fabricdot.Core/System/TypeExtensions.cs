using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;

// ReSharper disable once CheckNamespace
namespace System;

public static class TypeExtensions
{
    private static readonly ConcurrentDictionary<Type, string> _prettyPrintCache = new();

    private static readonly ConcurrentDictionary<Type, string> _typeCacheKeys = new();

    public static string PrettyPrint(this Type type)
    {
        Guard.Against.Null(type, nameof(type));

        return _prettyPrintCache.GetOrAdd(
            type,
            t =>
            {
                try
                {
                    return PrettyPrintRecursive(t, 0);
                }
                catch (Exception)
                {
                    return t.Name;
                }
            });
    }

    public static string GetCacheKey(this Type type)
    {
        Guard.Against.Null(type, nameof(type));

        return _typeCacheKeys.GetOrAdd(
            type,
            t => $"{t.PrettyPrint()}[hash: {t.GetHashCode()}]");
    }

    public static bool IsAssignableToGenericType(
        this Type givenType,
        Type genericType)
    {
        Guard.Against.Null(givenType, nameof(givenType));
        Guard.Against.Null(genericType, nameof(genericType));

        var interfaceTypes = givenType.GetInterfaces();

        if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
            return true;

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            return true;

        var baseType = givenType.BaseType;
        return baseType?.IsAssignableToGenericType(genericType) == true;
    }

    public static bool IsInNamespace(
        this Type type,
        string @namespace)
    {
        Guard.Against.Null(type, nameof(type));
        Guard.Against.NullOrWhiteSpace(@namespace, nameof(@namespace));

        var typeNamespace = type.Namespace ?? string.Empty;

        if (@namespace.Length > typeNamespace.Length)
            return false;

        var typeSubNamespace = typeNamespace[..@namespace.Length];

        if (typeSubNamespace.Equals(@namespace, StringComparison.Ordinal))
        {
            //exactly the same
            if (typeNamespace.Length == @namespace.Length)
                return true;

            //is a subnamespace?
            return typeNamespace[@namespace.Length] == '.';
        }

        return false;
    }

    public static bool IsInNamespaces(
        this Type type,
        IEnumerable<string> namespaces)
    {
        Guard.Against.Null(type, nameof(type));
        Guard.Against.NullOrEmpty(namespaces, nameof(namespaces));

        return namespaces.Any(v => type.IsInNamespace(v));
    }

    public static bool IsNonAbstractClass(
        this Type type,
        bool publicOnly)
    {
        Guard.Against.Null(type, nameof(type));

        if (type.IsSpecialName)
            return false;

        if (type.IsClass && !type.IsAbstract)
        {
            if (type.IsDefined<CompilerGeneratedAttribute>(true))
                return false;

            if (publicOnly)
                return type.IsPublic || type.IsNestedPublic;

            return true;
        }

        return false;
    }

    private static string PrettyPrintRecursive(Type type, int depth)
    {
        if (depth > 3)
            return type.Name;

        var nameParts = type.Name.Split('`');
        if (nameParts.Length == 1)
            return nameParts[0];

        var genericArguments = type.GetTypeInfo().GetGenericArguments();
        return !type.IsConstructedGenericType
            ? $"{nameParts[0]}<{new string(',', genericArguments.Length - 1)}>"
            : $"{nameParts[0]}<{string.Join(",", genericArguments.Select(t => PrettyPrintRecursive(t, depth + 1)))}>";
    }
}