using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Add the <paramref name="dependencyRegistrar" /> to <paramref name="services" />
    /// </summary>
    /// <param name="services"></param>
    /// <param name="dependencyRegistrar"></param>
    public static void AddDependencyRegistrar(
        this IServiceCollection services,
        IDependencyRegistrar dependencyRegistrar)
    {
        Guard.Against.Null(services, nameof(services));
        Guard.Against.Null(dependencyRegistrar, nameof(dependencyRegistrar));

        services.AddDependencyRegistrarCollection();
        var dependencyRegistrars = services.GetRequiredSingletonInstance<IDependencyRegistrarCollection>();
        dependencyRegistrars.AddIfNotContains(dependencyRegistrar);
    }

    /// <summary>
    ///     Add the <typeparamref name="TDependencyRegistrar" /> to <paramref name="services" />
    /// </summary>
    /// <typeparam name="TDependencyRegistrar"></typeparam>
    /// <param name="services"></param>
    public static void AddDependencyRegistrar<TDependencyRegistrar>(this IServiceCollection services) where TDependencyRegistrar : IDependencyRegistrar, new()
    {
        services.AddDependencyRegistrar(new TDependencyRegistrar());
    }

    /// <summary>
    ///     Add the <paramref name="implementationType" /> to <paramref name="services" /> with
    ///     <see cref="IDependencyRegistrar" />
    /// </summary>
    /// <param name="services"></param>
    /// <param name="implementationType"></param>
    public static void AddType(
        this IServiceCollection services,
        Type implementationType)
    {
        Guard.Against.Null(services, nameof(services));
        Guard.Against.Null(implementationType, nameof(implementationType));

        foreach (var registrar in services.GetRequiredSingletonInstance<IDependencyRegistrarCollection>())
        {
            registrar.Register(services, implementationType);
        }
    }

    private static void AddDependencyRegistrarCollection(this IServiceCollection services)
    {
        Guard.Against.Null(services, nameof(services));

        if (services.ContainsService<IDependencyRegistrarCollection>())
            return;

        services.AddSingleton<IDependencyRegistrarCollection>(new DependencyRegistrarCollection());
    }
}