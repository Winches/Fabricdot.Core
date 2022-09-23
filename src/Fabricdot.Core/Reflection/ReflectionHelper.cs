using System.Reflection;
using Ardalis.GuardClauses;

namespace Fabricdot.Core.Reflection;

/// <summary>
///     Reflection Helper
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
    ///     Find derived types by given type.
    /// </summary>
    /// <remarks>Abstract class and interface will be ignore.</remarks>
    /// <typeparam name="TFind">base type</typeparam>
    /// <param name="assemblies">target assemblies</param>
    public static List<Type> FindTypes<TFind>(params Assembly[] assemblies)
    {
        var findType = typeof(TFind);
        return FindTypes(findType, assemblies);
    }

    /// <summary>
    ///     Find derived types by given type.
    /// </summary>
    /// <remarks>Abstract class and interface will be ignore.</remarks>
    /// <param name="findType">base type</param>
    /// <param name="assemblies">target assemblies</param>
    public static List<Type> FindTypes(Type findType, params Assembly[] assemblies)
    {
        var result = new List<Type>();
        foreach (var assembly in assemblies)
            result.AddRange(FindTypes(findType, assembly));
        return result.Distinct().ToList();
    }

    private static IEnumerable<Type> FindTypes(Type findType, Assembly assembly)
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
        {
            if (type.IsInterface || type.IsAbstract)
                continue;
            if (findType.IsAssignableFrom(type) || type.IsAssignableToGenericType(findType))
                result.Add(type);
        }

        return result;
    }

    public static IReadOnlyCollection<Type> GetTypes(Assembly assembly)
    {
        Guard.Against.Null(assembly, nameof(assembly));

        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(v => v is not null).ToArray()!;
        }
    }
}