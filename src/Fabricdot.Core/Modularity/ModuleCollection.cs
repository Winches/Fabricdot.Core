using System.Collections.ObjectModel;
using Ardalis.GuardClauses;

namespace Fabricdot.Core.Modularity;

public class ModuleCollection : Collection<IModuleMetadata>, IModuleCollection
{
    public virtual void Build()
    {
        SolveDependencies();
        Sort();
    }

    public virtual void Sort()
    {
        var sortedModules = this.TopologicalSort(m => m.Dependencies);
        Clear();
        AddRange(sortedModules);
    }

    protected virtual void SolveDependencies()
    {
        foreach (var module in this)
        {
            SetDependencies(module);
        }
    }

    protected virtual void SetDependencies(IModuleMetadata module)
    {
        foreach (var dependentModuleType in module.GetDependentModuleTypes())
        {
            var dependedModule = this.FirstOrDefault(m => m.Type == dependentModuleType);
            if (dependedModule == null)
            {
                throw new ModularityException(
                    $"Could not find a dependent module {dependentModuleType.AssemblyQualifiedName} for {module.Type.AssemblyQualifiedName}");
            }

            module.AddDependency(dependedModule);
        }
    }

    protected virtual void AddRange(IEnumerable<IModuleMetadata> items)
    {
        Guard.Against.Null(items, nameof(items));

        foreach (var module in items)
            Add(module);
    }
}