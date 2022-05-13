using System.ComponentModel;
using System.Globalization;
using Ardalis.GuardClauses;

namespace System
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object obj) => obj is null;

        /// <summary>
        ///     ( <typeparamref name="T" />) <paramref name="obj" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Cast<T>(this object obj) => (T)obj;

        /// <summary>
        ///     <paramref name="obj" /> as <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T As<T>(this object obj) => obj is T to ? to : default;

        /// <summary>
        ///     Converts object to a value or enum type using <see
        ///     cref="Convert.ChangeType(object,TypeCode)" /> , <see cref="Enum.Parse(Type,string)"
        ///     /> or <see cref="Cast{T}(object)" />.
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T To<T>(this object value)
        {
            Guard.Against.Null(value, nameof(value));

            var type = typeof(T);
            if (type == typeof(Guid))
            {
                return TypeDescriptor.GetConverter(type)
                                     .ConvertFromInvariantString(value.ToString())
                                     .Cast<T>();
            }

            if (type.IsEnum)
            {
                return Enum.Parse(type, value.ToString())
                           .Cast<T>();
            }

            return Convert.ChangeType(value, type, CultureInfo.InvariantCulture)
                          .Cast<T>();
        }
    }
}