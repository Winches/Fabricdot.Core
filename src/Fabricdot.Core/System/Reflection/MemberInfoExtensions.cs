// ReSharper disable once CheckNamespace

namespace System.Reflection
{
    public static class MemberInfoExtensions
    {
        /// <summary>
        ///     Gets a member indicating whether the type of Boolean.
        /// </summary>
        /// <param name="member">member</param>
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
        ///     Gets a member indicating whether the type of collection.
        /// </summary>
        /// <param name="member">member</param>
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
        ///     Gets a type indicating whether the type of DateTime.
        /// </summary>
        /// <param name="member">member</param>
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
        ///     Gets a type indicating whether the type of Integer.
        /// </summary>
        /// <param name="member">member</param>
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
        ///     Gets a type indicating whether the type of Number.
        /// </summary>
        /// <param name="member">member</param>
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