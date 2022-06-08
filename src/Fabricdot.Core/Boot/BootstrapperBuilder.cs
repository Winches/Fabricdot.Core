using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Fabricdot.Core.Configuration;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Core.Modularity;
using Fabricdot.Core.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fabricdot.Core.Boot;

internal sealed class BootstrapperBuilder : IBootstrapperBuilder
{
    private readonly BootstrapperBuilderOptions _options;

    public IServiceCollection Services => _options.Services;

    public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

    public BootstrapperBuilder(BootstrapperBuilderOptions options)
    {
        _options = Guard.Against.Null(options, nameof(options));

        AddCoreServices(options.ConfigurationOptions);
    }

    public IBootstrapperBuilder AddModules(Type moduleType)
    {
        Guard.Against.Null(moduleType, nameof(moduleType));

        if (Services.ContainsService<IModuleCollection>())
            return this;

        var modules = Services.GetRequiredSingletonInstance<IModuleLoader>()
                              .LoadModules(moduleType);
        Services.AddSingleton(modules);
        Services.GetRequiredSingletonInstance<IModuleServiceVisitor>()
                .Visit(modules, Services);

        return this;
    }

    public IApplication Build(IServiceProvider serviceProvider)
    {
        var app = new Bootstrapper();
        app.SetServiceProvider(serviceProvider);
        Services.AddSingleton<IApplication>(app);
        return app;
    }

    private void AddCoreServices(ConfigurationBuilderOptions configurationOptions)
    {
        Services.AddSingleton<IBootstrapperBuilder>(this);
        Services.TryAddConfiguration(configurationOptions);
        Services.AddLogging();
        Services.AddOptions();

        // modularity services
        Services.AddDependencyRegistrar<DefaultDependencyRegistrar>();
        Services.TryAddSingleton<IModuleLoader>(new ModuleLoader());
        Services.TryAddSingleton<IModuleServiceVisitor>(new ModuleServiceVisitor(new ConfigureServiceContextFactory(this)));
        Services.TryAddSingleton<IModuleManager, ModuleManager>();

        var types = ReflectionHelper.GetTypes(GetType().Assembly);
        types.ForEach(type => Services.AddType(type));
    }
}