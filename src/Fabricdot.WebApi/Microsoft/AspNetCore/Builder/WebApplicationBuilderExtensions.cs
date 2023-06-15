using Ardalis.GuardClauses;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Fabricdot.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    ///     Create a modularity application with the specific <paramref name="builder" />.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="moduleType"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddBootstrapper(
        this WebApplicationBuilder builder,
        Type moduleType,
        Action<BootstrapperBuilderOptions>? configureOptions = null)
    {
        Guard.Against.Null(builder, nameof(builder));
        Guard.Against.Null(moduleType, nameof(moduleType));

        var options = new BootstrapperBuilderOptions(builder.Services);
        configureOptions?.Invoke(options);

        Bootstrapper.CreateBuilder(options).AddModules(moduleType);

        return builder;
    }

    /// <summary>
    ///     Create a modularity application with the specific <paramref name="builder" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddBootstraper<T>(
        this WebApplicationBuilder builder,
        Action<BootstrapperBuilderOptions>? configureOptions = null) where T : class, IModule
    {
        builder.Services.AddBootstrapper<T>(configureOptions);
        return builder;
    }

    /// <summary>
    ///     Override the factory used to create service provider.
    /// </summary>
    /// <typeparam name="TContainerBuilder"></typeparam>
    /// <param name="builder"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    public static WebApplicationBuilder UseServiceProviderFactory<TContainerBuilder>(
        this WebApplicationBuilder builder,
        IServiceProviderFactory<TContainerBuilder> factory) where TContainerBuilder : notnull
    {
        builder.Host.UseServiceProviderFactory(factory);
        return builder;
    }

    /// <summary>
    ///     Override the factory used to create service provider with <see
    ///     cref="FabricdotServiceProviderFactory" />.
    /// </summary>
    /// <typeparam name="TContainerBuilder"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder UseServiceProviderFactory<TContainerBuilder>(this WebApplicationBuilder builder) where TContainerBuilder : notnull
    {
        builder.Host.UseServiceProviderFactory(new FabricdotServiceProviderFactory());
        return builder;
    }
}
