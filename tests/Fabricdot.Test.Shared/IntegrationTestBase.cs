using System;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Test.Shared;

public abstract class IntegrationTestBase : IDisposable
{
    protected IServiceProviderFactory<IServiceCollection> ServiceProviderFactory { get; set; } = new DefaultServiceProviderFactory();

    protected IServiceProvider RootServiceProvider { get; set; }

    protected IServiceProvider ServiceProvider => ServiceScope.ServiceProvider;

    protected IServiceScope ServiceScope { get; set; }

    protected IntegrationTestBase()
    {
        Initialize();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Initialize()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        RootServiceProvider = ServiceProviderFactory?.CreateServiceProvider(serviceCollection)
            ?? serviceCollection.BuildServiceProvider();
        ServiceScope = ServiceProvider.CreateScope();
    }

    protected abstract void ConfigureServices(IServiceCollection serviceCollection);

    protected void UseServiceProviderFactory<TFactory>() where TFactory : IServiceProviderFactory<IServiceCollection>, new()
    {
        ServiceProviderFactory = new TFactory();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            ServiceScope?.Dispose();
    }
}

public abstract class IntegrationTestBase<TModule> : IntegrationTestBase where TModule : class, IModule
{
    protected override void Initialize()
    {
        var services = new ServiceCollection();
        var app = services.AddBootstrapper<TModule>();
        ConfigureServices(services);

        RootServiceProvider = ServiceProviderFactory.CreateServiceProvider(services);
        ServiceScope = RootServiceProvider.CreateScope();

        app.Build(ServiceScope.ServiceProvider);

        RootServiceProvider.BootstrapAsync().GetAwaiter().GetResult();
    }

    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
    }
}