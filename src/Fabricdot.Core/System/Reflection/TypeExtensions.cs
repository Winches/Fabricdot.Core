using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace System.Reflection
{
    public static class TypeExtensions
    {
        public static bool IsAssignableToGenericType(
            this Type givenType,
            Type genericType)
        {
            if (givenType == null)
                throw new ArgumentNullException(nameof(givenType));
            if (genericType == null)
                throw new ArgumentNullException(nameof(genericType));

            var interfaceTypes = givenType.GetInterfaces();

            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
                return true;

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = givenType.BaseType;
            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }

        public static bool IsAssignableTo<T>(this Type type)
        {
            return typeof(T).GetTypeInfo().IsAssignableFrom(type);
        }

        /// <summary>
        ///     Get type of top base class by given type.
        /// </summary>
        /// <param name="type">type</param>
        public static Type GetTopBaseType(this Type type)
        {
            while (true)
            {
                if (type == null)
                    return null;
                if (type.IsInterface)
                    return type;
                if (type.BaseType == typeof(object))
                    return type;
                type = type.BaseType!;
            }
        }

        public static bool IsInNamespace(
            this Type type,
            string @namespace)
        {
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
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (namespaces is null)
                throw new ArgumentNullException(nameof(namespaces));

            return namespaces.Any(v => type.IsInNamespace(v));
        }

        public static bool IsNonAbstractClass(
            this Type type,
            bool publicOnly)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

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
    }
}