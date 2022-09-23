using Fabricdot.Core.Boot;

namespace Fabricdot.Core.Modularity;

public abstract class ModuleBase : IModule, IConfigureServices, IApplicationStarting, IApplicationStopping
{
    protected ModuleBase()
    {
    }

    public virtual void ConfigureServices(ConfigureServiceContext context)
    {
    }

    public virtual Task OnStartingAsync(ApplicationStartingContext context)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnStoppingAsync(ApplicationStoppingContext context)
    {
        return Task.CompletedTask;
    }
}