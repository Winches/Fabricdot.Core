using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fabricdot.Common.Core.Reflections
{
    public static class AttributeExtensions
    {
        /// <summary>
        ///     get attributes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetAttributes<T>(this Type type, bool inherit = false) where T : class
        {
            return type.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }

        /// <summary>
        ///     get attributes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetAttributes<T>(this MemberInfo type, bool inherit = false) where T : class
        {
            return type.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }
    }
}