using System.Diagnostics;
using Ardalis.GuardClauses;

namespace Fabricdot.Core.Modularity;

public class ModuleLoader : IModuleLoader
{
    public IModuleCollection LoadModules(Type startupModuleType)
    {
        Guard.Against.InvalidModuleType(startupModuleType, nameof(startupModuleType));

        var modules = CreateModules(startupModuleType);
        return BuildModules(modules, startupModuleType);
    }

    protected virtual IModuleCollection CreateModules(Type startupModuleType)
    {
        var modules = new ModuleCollection();
        foreach (var moduleType in GetModuleTypesRecursively(startupModuleType))
        {
            var moduleMetadata = CreateModuleMetadata(moduleType);
            modules.Add(moduleMetadata);
        }
        return modules;
    }

    protected virtual IModuleMetadata CreateModuleMetadata(Type moduleType)
    {
        var instance = (IModule?)Activator.CreateInstance(moduleType);
        return new ModuleMetadata(moduleType, instance!);
    }

    protected virtual IReadOnlySet<Type> GetModuleTypesRecursively(Type moduleType)
    {
        Guard.Against.InvalidModuleType(moduleType, nameof(moduleType));

        var moduleTypes = new HashSet<Type>();
        LoadDependentModules(moduleTypes, moduleType);
        return moduleTypes;

        static void LoadDependentModules(
            ISet<Type> pool,
            Type moduleType,
            int depth = 0)
        {
            Guard.Against.InvalidModuleType(moduleType, nameof(moduleType));

            if (!pool.Add(moduleType))
                return;

            Debug.WriteLine($"{new string('-', depth)} {moduleType.PrettyPrint()}");
            foreach (var dependentModuleType in moduleType.GetDependentModuleTypes())
            {
                LoadDependentModules(pool, dependentModuleType, depth + 1);
            }
        }
    }

    protected virtual IModuleCollection BuildModules(
        IModuleCollection modules,
        Type startupModuleType)
    {
        modules.Build();
        modules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
        return modules;
    }
}
