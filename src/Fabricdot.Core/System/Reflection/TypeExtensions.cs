using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System.Reflection
{
    public static class TypeExtensions
    {
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
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

        /// <summary>
        ///     Gets a type indicating whether the type of collection.
        /// </summary>
        /// <param name="type">type</param>
        public static bool IsCollection(this Type type)
        {
            return type.IsArray || IsGenericCollection(type);
        }

        /// <summary>
        ///     Gets a type indicating whether the type of generic collection.
        /// </summary>
        /// <param name="type">类型</param>
        public static bool IsGenericCollection(this Type type)
        {
            if (!type.IsGenericType)
                return false;
            var typeDefinition = type.GetGenericTypeDefinition();
            return typeDefinition == typeof(IEnumerable<>)
                   || typeDefinition == typeof(IReadOnlyCollection<>)
                   || typeDefinition == typeof(IReadOnlyList<>)
                   || typeDefinition == typeof(ICollection<>)
                   || typeDefinition == typeof(IList<>)
                   || typeDefinition == typeof(List<>);
        }

        internal static bool HasConstructorParameterOfType(this Type type, Predicate<Type> predicate)
        {
            return type.GetTypeInfo().GetConstructors()
                .Any(c => c.GetParameters()
                    .Any(p => predicate(p.ParameterType)));
        }
    }
}