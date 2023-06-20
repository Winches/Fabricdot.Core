using System.Collections.Concurrent;
using System.Reflection;
using Ardalis.GuardClauses;

namespace Fabricdot.Core.Modularity;

internal static class ModuleMetadataExtensions
{
    private static readonly ConcurrentDictionary<Type, IReadOnlySet<Type>> s_dependentTypeCache = new();

    internal static IReadOnlySet<Type> GetDependentModuleTypes(this IModuleMetadata module)
    {
        Guard.Against.Null(module, nameof(module));

        return module.Type.GetDependentModuleTypes();
    }

    internal static IReadOnlySet<Type> GetDependentModuleTypes(this Type moduleType)
    {
        Guard.Against.Null(moduleType, nameof(moduleType));

        if (s_dependentTypeCache.ContainsKey(moduleType))
        {
            return s_dependentTypeCache[moduleType];
        }
        else
        {
            var types = moduleType.GetCustomAttributes<RequiresAttribute>()
                                  .SelectMany(v => v.Requires)
                                  .ToHashSet();
            s_dependentTypeCache.TryAdd(moduleType, types);
            return types;
        }
    }
}
