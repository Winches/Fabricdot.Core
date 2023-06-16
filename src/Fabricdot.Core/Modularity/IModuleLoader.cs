namespace Fabricdot.Core.Modularity;

public interface IModuleLoader
{
    IModuleCollection LoadModules(Type startupModuleType);
}
