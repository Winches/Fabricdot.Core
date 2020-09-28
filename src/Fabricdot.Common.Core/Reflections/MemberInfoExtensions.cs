using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fabricdot.Common.Core.Reflections
{
    public static class MemberInfoExtensions
    {
        /// <summary>
        ///     获取顶级基类
        /// </summary>
        /// <param name="type">类型</param>
        public static Type GetTopBaseType(this Type type)
        {
            while (true)
            {
                if (type == null) return null;
                if (type.IsInterface) return type;
                if (type.BaseType == typeof(object)) return type;
                type = type.BaseType!;
            }
        }

        /// <summary>
        ///     是否集合
        /// </summary>
        /// <param name="type">类型</param>
        public static bool IsCollection(this Type type)
        {
            return type.IsArray || IsGenericCollection(type);
        }

        /// <summary>
        ///     是否泛型集合
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

        /// <summary>
        ///     是否布尔类型
        /// </summary>
        /// <param name="member">成员</param>
        public static bool IsBool(this MemberInfo member)
        {
            if (member == null)
                return false;
            return member.MemberType switch
            {
                MemberTypes.TypeInfo => member.ToString() == "System.Boolean",
                MemberTypes.Property => IsBool((PropertyInfo) member),
                _ => false
            };
        }

        /// <summary>
        ///     是否枚举类型
        /// </summary>
        /// <param name="member">成员</param>
        public static bool IsEnum(this MemberInfo member)
        {
            if (member == null)
                return false;
            return member.MemberType switch
            {
                MemberTypes.TypeInfo => ((TypeInfo) member).IsEnum,
                MemberTypes.Property => IsEnum((PropertyInfo) member),
                _ => false
            };
        }

        /// <summary>
        ///     是否日期类型
        /// </summary>
        /// <param name="member">成员</param>
        public static bool IsDateTime(this MemberInfo member)
        {
            if (member == null)
                return false;
            return member.MemberType switch
            {
                MemberTypes.TypeInfo => member.ToString() == "System.DateTime",
                MemberTypes.Property => IsDateTime((PropertyInfo) member),
                _ => false
            };
        }

        /// <summary>
        ///     是否整型
        /// </summary>
        /// <param name="member">成员</param>
        public static bool IsInt(this MemberInfo member)
        {
            if (member == null)
                return false;
            return member.MemberType switch
            {
                MemberTypes.TypeInfo => member.ToString() == "System.Int32" || member.ToString() == "System.Int16" ||
                                        member.ToString() == "System.Int64",
                MemberTypes.Property => IsInt((PropertyInfo) member),
                _ => false
            };
        }

        /// <summary>
        ///     是否数值类型
        /// </summary>
        /// <param name="member">成员</param>
        public static bool IsNumber(this MemberInfo member)
        {
            if (member == null)
                return false;
            if (IsInt(member))
                return true;
            return member.MemberType switch
            {
                MemberTypes.TypeInfo => member.ToString() == "System.Double" || member.ToString() == "System.Decimal" ||
                                        member.ToString() == "System.Single",
                MemberTypes.Property => IsNumber((PropertyInfo) member),
                _ => false
            };
        }

        private static bool IsBool(PropertyInfo property)
        {
            return property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?);
        }

        private static bool IsEnum(PropertyInfo property)
        {
            if (property.PropertyType.GetTypeInfo().IsEnum)
                return true;
            var value = Nullable.GetUnderlyingType(property.PropertyType);
            return value != null && value.GetTypeInfo().IsEnum;
        }

        private static bool IsDateTime(PropertyInfo property)
        {
            if (property.PropertyType == typeof(DateTime))
                return true;
            if (property.PropertyType == typeof(DateTime?))
                return true;
            return false;
        }

        private static bool IsInt(PropertyInfo property)
        {
            if (property.PropertyType == typeof(int))
                return true;
            if (property.PropertyType == typeof(int?))
                return true;
            if (property.PropertyType == typeof(short))
                return true;
            if (property.PropertyType == typeof(short?))
                return true;
            if (property.PropertyType == typeof(long))
                return true;
            if (property.PropertyType == typeof(long?))
                return true;
            return false;
        }

        private static bool IsNumber(PropertyInfo property)
        {
            if (property.PropertyType == typeof(double))
                return true;
            if (property.PropertyType == typeof(double?))
                return true;
            if (property.PropertyType == typeof(decimal))
                return true;
            if (property.PropertyType == typeof(decimal?))
                return true;
            if (property.PropertyType == typeof(float))
                return true;
            if (property.PropertyType == typeof(float?))
                return true;
            return false;
        }
    }
}