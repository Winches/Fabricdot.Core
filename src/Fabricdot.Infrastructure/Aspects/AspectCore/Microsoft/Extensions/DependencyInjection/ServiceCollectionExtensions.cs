using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ardalis.GuardClauses;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Fabricdot.Core.Aspects;
using Fabricdot.Infrastructure.Aspects;
using Fabricdot.Infrastructure.Aspects.AspectCore;
using JetBrains.Annotations;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

[UsedImplicitly]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInterceptors(
        this IServiceCollection serviceCollection,
        Action<IInterceptorOptions>? optionBuilderAction = null)
    {
        Guard.Against.Null(serviceCollection, nameof(serviceCollection));

        var options = BuildInterceptorOptions(serviceCollection, optionBuilderAction);
        serviceCollection.AddTransient(typeof(AspectCoreInterceptorAdapter<>));

        serviceCollection.ConfigureDynamicProxy(
            config =>
            {
                foreach (var interceptorDescriptor in options.Interceptors.OrderBy(v => v.Order))
                {
                    var adapterType = AdaptInterceptorType(interceptorDescriptor.InterceptorType);
                    if (adapterType == null)
                        return;

                    config.Interceptors.AddServiced(adapterType, CreateInterceptorPredicate(interceptorDescriptor));
                }

                config.NonAspectPredicates.Add(AspectPredicateUtil.IsDefined<DisableAspectAttribute>(true));
                foreach (var excludeTarget in options.ExcludeTargets)
                {
                    config.NonAspectPredicates.AddService(excludeTarget.FullName);
                }
            });
        return serviceCollection;
    }

    public static IServiceProvider BuildProxiedServiceProvider(this IServiceCollection serviceCollection)
    {
        Guard.Against.Null(serviceCollection, nameof(serviceCollection));
        return serviceCollection.BuildDynamicProxyProvider();
    }

    private static InterceptorOptions BuildInterceptorOptions(
        IServiceCollection serviceCollection,
        Action<IInterceptorOptions>? optionBuilderAction)
    {
        var options = new InterceptorOptions();
        var interceptorTypes = GetInterceptorTypes(serviceCollection).ToList();
        interceptorTypes.ForEach(v => options.Interceptors.Add(v));
        optionBuilderAction?.Invoke(options);
        return options;
    }

    private static IEnumerable<Type> GetInterceptorTypes(IServiceCollection serviceCollection)
    {
        var interceptorTypes = serviceCollection
                               .Where(v => v.ServiceType.IsAssignableTo(typeof(IInterceptor)))
                               .Where(v => v.ServiceType.IsInterface || v.ServiceType.IsClass)
                               .Select(v => v.ServiceType)
                               .Distinct();
        return interceptorTypes;
    }

    [CanBeNull]
    private static Type? AdaptInterceptorType(Type interceptorType)
    {
        return interceptorType.IsAssignableTo(typeof(IInterceptor))
            ? typeof(AspectCoreInterceptorAdapter<>).MakeGenericType(interceptorType)
            : null;
    }

    private static AspectPredicate CreateInterceptorPredicate(InterceptorDescriptor interceptorDescriptor)
    {
        var predicate = AspectPredicateUtil.CreatePredicate(interceptorDescriptor);
#if DEBUG
        bool Adaptar(MethodInfo methodInfo)
        {
            var ret = predicate(methodInfo);
            return ret;
        }
        return Adaptar;
#else
        return predicate;
#endif
    }
}