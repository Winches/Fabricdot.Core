using System.Reflection;
using Ardalis.GuardClauses;

namespace Fabricdot.Core.Modularity;

public class ModuleMetadata : IModuleMetadata
{
    private readonly HashSet<IModuleMetadata> _dependencies = new();

    public string Name { get; }

    public Type Type { get; }

    public Assembly Assembly { get; }

    public IReadOnlySet<IModuleMetadata> Dependencies => _dependencies;

    public IModule Instance { get; }

    public ModuleMetadata(
        string name,
        Type type,
        IModule instance,
        params IModuleMetadata[] modules)
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        Type = Guard.Against.InvalidModuleType(type, nameof(type));
        Instance = Guard.Against.Null(instance, nameof(instance));
        Assembly = Type.Assembly;

        if (Instance.GetType() != Type)
            throw new ArgumentException("Instance type does not match with module type.");

        if (modules != null)
        {
            foreach (var module in modules)
            {
                AddDependency(module);
            }
        }
    }

    public ModuleMetadata(
        Type type,
        IModule instance) : this(
            type.FullName!,
            type,
            instance,
            Array.Empty<IModuleMetadata>())
    {
    }

    public virtual void AddDependency(IModuleMetadata module)
    {
        Guard.Against.Null(module, nameof(module));
        _dependencies.Add(module);
    }

    public override int GetHashCode() => Name.GetHashCode();

    public override bool Equals(object? obj) => Equals(obj as IModuleMetadata);

    public virtual bool Equals(IModuleMetadata? obj) => Name.Equals(obj?.Name);
}