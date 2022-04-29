using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fabricdot.Core.Modularity
{
    public interface IModuleMetadata
    {
        string Name { get; }

        Type Type { get; }

        Assembly Assembly { get; }

        IModule Instance { get; }

        IReadOnlySet<IModuleMetadata> Dependencies { get; }

        void AddDependency(IModuleMetadata module);
    }
}