using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Fabricdot.Core.Reflection
{
    /// <summary>
    ///     反射操作
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        ///     Get assembly from given name.
        /// </summary>
        /// <param name="assemblyName">assembly name</param>
        public static Assembly GetAssembly(string assemblyName)
        {
            return Assembly.Load(new AssemblyName(assemblyName));
        }

        /// <summary>
        ///     Get assemblies from given directory.
        /// </summary>
        /// <param name="directoryPath">absolutely path</param>
        public static List<Assembly> GetAssemblies(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                .ToList()
                .Where(t => t.EndsWith(".exe") || t.EndsWith(".dll"))
                .Select(path => Assembly.Load(new AssemblyName(path)))
                .ToList();
        }

        /// <summary>
        ///     Find derived types by given type.Base abstract class is not supported.
        /// </summary>
        /// <typeparam name="TFind">base type</typeparam>
        /// <param name="assemblies">target assemblies</param>
        public static List<Type> FindTypes<TFind>(params Assembly[] assemblies)
        {
            var findType = typeof(TFind);
            return FindTypes(findType, assemblies);
        }

        /// <summary>
        ///     Find derived types by given type.Base abstract class is not supported.
        /// </summary>
        /// <param name="findType">base type</param>
        /// <param name="assemblies">target assemblies</param>
        public static List<Type> FindTypes(Type findType, params Assembly[] assemblies)
        {
            var result = new List<Type>();
            foreach (var assembly in assemblies)
                result.AddRange(GetTypes(findType, assembly));
            return result.Distinct().ToList();
        }

        private static IEnumerable<Type> GetTypes(Type findType, Assembly assembly)
        {
            var result = new List<Type>();
            if (assembly == null)
                return result;
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException)
            {
                return result;
            }

            foreach (var type in types)
                AddType(result, findType, type);
            return result;
        }

        private static void AddType(
            ICollection<Type> result,
            Type findType,
            Type type)
        {
            if (type.IsInterface || type.IsAbstract)
                return;
            if (findType.IsAssignableFrom(type) == false && MatchGeneric(findType, type) == false)
                return;
            result.Add(type);
        }

        private static bool MatchGeneric(Type findType, Type type)
        {
            if (findType.IsGenericTypeDefinition == false)
                return false;
            var definition = findType.GetGenericTypeDefinition();
            foreach (var implementedInterface in type.FindInterfaces((_, _) => true, null))
            {
                if (implementedInterface.IsGenericType == false)
                    continue;
                return definition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
            }

            return false;
        }
    }
}