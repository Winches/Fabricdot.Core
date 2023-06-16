using Fabricdot.Core.Boot;

namespace Fabricdot.Core.Modularity;

public interface IModuleManager
{
    IReadOnlyCollection<IModuleMetadata> Modules { get; }

    Task StartAsync(ApplicationStartingContext context);

    Task StopAsync(ApplicationStoppingContext context);
}
