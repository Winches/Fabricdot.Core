using System;

namespace Fabricdot.Core.Modularity;

public interface IModuleLoader
{
    IModuleCollection LoadModules(Type startupModuleType);
}