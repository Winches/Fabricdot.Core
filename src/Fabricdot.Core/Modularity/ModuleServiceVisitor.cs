using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ardalis.GuardClauses;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Core.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Modularity;

public class ModuleServiceVisitor : IModuleServiceVisitor
{
    private readonly HashSet<Type> _visitedTypes = new();
    private readonly IConfigureServiceContextFactory _configureServiceContextFactory;

    public ModuleServiceVisitor(IConfigureServiceContextFactory configureServiceContextFactory)
    {
        Guard.Against.Null(configureServiceContextFactory, nameof(configureServiceContextFactory));
        _configureServiceContextFactory = configureServiceContextFactory;
    }

    public virtual void Visit(
        IModuleCollection modules,
        IServiceCollection services)
    {
        Guard.Against.Null(modules, nameof(modules));
        Guard.Against.Null(services, nameof(services));

        var context = _configureServiceContextFactory.Create();
        foreach (var module in modules)
        {
            if (module.Instance is IPreConfigureService preConfigureService)
                preConfigureService.PreConfigureServices(context);

            ConfigureExports(services, module, _visitedTypes);

            ConfigureServices(module, context);

            if (module.Instance is IPostConfigureService postConfigureService)
                postConfigureService.PostConfigureServices(context);
        }
    }

    private static void ConfigureExports(
        IServiceCollection services,
        IModuleMetadata module,
        ISet<Type> visitedTypes)
    {
        if (!module.Type.IsDefined(typeof(ExportsAttribute), false))
            return;

        var types = ReflectionHelper.GetTypes(module.Assembly);
        var exports = module.Type.GetCustomAttributes<ExportsAttribute>(false)
                                 .SelectMany(v => v.Exports)
                                 .Distinct()
                                 .ToArray();

        types.ForEach(type =>
        {
            if (visitedTypes.Contains(type))
                return;

            if (exports.IsNullOrEmpty() || type.IsInNamespaces(exports))
            {
                services.AddType(type);
                visitedTypes.Add(type);
            }
        });
    }

    private static void ConfigureServices(
        IModuleMetadata module,
        ConfigureServiceContext context)
    {
        try
        {
            (module.Instance as IConfigureServices)?.ConfigureServices(context);
        }
        catch (Exception ex)
        {
            throw new ModularityException(
                $"An error occurred during {nameof(IConfigureServices.ConfigureServices)} phase of the module {module.Name}.", ex);
        }
    }
}