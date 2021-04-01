using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fabricdot.Core.Reflection
{
    /// <summary>
    ///     Type finder
    /// </summary>
    public interface ITypeFinder
    {
        /// <summary>
        ///     Get assemblies
        /// </summary>
        List<Assembly> GetAssemblies();

        /// <summary>
        ///     Get implemented types
        /// </summary>
        /// <typeparam name="T">find type</typeparam>
        /// <param name="assemblies">assemblies</param>
        List<Type> Find<T>(params Assembly[] assemblies);

        /// <summary>
        ///     Get implemented types
        /// </summary>
        /// <param name="findType">find type</param>
        /// <param name="assemblies">assemblies</param>
        List<Type> Find(Type findType, params Assembly[] assemblies);
    }
}