using System.Collections.Generic;

namespace Fabricdot.Core.Modularity;

public interface IModuleCollection : IList<IModuleMetadata>
{
    void Build();
}