using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Core.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Boot;

public class Bootstrapper : Application, ISupportSetServiceProvider
{
    private bool _started;
    private bool _stopped;

    internal Bootstrapper()
    {
    }

    public static IBootstrapperBuilder CreateBuilder(BootstrapperBuilderOptions options)
    {
        return new BootstrapperBuilder(options);
    }

    public override async Task StartAsync(CancellationToken cancellationToken = default)
    {
        if (_started)
            throw new InvalidOperationException("Application have already been started!");

        await StartModulesAsync(cancellationToken);
        _started = true;
    }

    public override async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_stopped)
            throw new InvalidOperationException("Application have already been stoppted!");
        if (!_started)
            throw new InvalidOperationException("Application has not been started!");

        await StopModulesAsync(cancellationToken);
        _stopped = true;
    }

    public virtual void SetServiceProvider(IServiceProvider serviceProvider)
    {
        if (Services != null)
            throw new InvalidOperationException("Service provider already exists.");

        Services = Guard.Against.Null(serviceProvider, nameof(serviceProvider));
    }

    protected virtual async Task StartModulesAsync(CancellationToken cancellationToken = default)
    {
        using var scope = Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<IModuleManager>()
                                   .StartAsync(new ApplicationStartingContext(scope.ServiceProvider));
    }

    protected virtual async Task StopModulesAsync(CancellationToken cancellationToken = default)
    {
        using var scope = Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<IModuleManager>()
                                   .StopAsync(new ApplicationStoppingContext(scope.ServiceProvider));
    }
}