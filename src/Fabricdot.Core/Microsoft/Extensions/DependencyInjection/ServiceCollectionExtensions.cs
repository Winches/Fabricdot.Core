using Ardalis.GuardClauses;
using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static bool ContainsService(
        this IServiceCollection services,
        Type type)
    {
        Guard.Against.Null(services, nameof(services));

        return services.Any(v => v.ServiceType == type);
    }

    public static bool ContainsService<T>(this IServiceCollection services) => services.ContainsService(typeof(T));

    public static T? GetSingletonInstance<T>(this IServiceCollection services)
    {
        Guard.Against.Null(services, nameof(services));

        var serviceDescriptor = services.SingleOrDefault(v => v.ImplementationInstance is T);
        return (T?)serviceDescriptor?.ImplementationInstance;
    }

    public static T GetRequiredSingletonInstance<T>(this IServiceCollection services)
    {
        return services.GetSingletonInstance<T>()
               ?? throw new InvalidOperationException($"Could not find {typeof(T).PrettyPrint()} in service collection.");
    }

    public static void Add(
        this IServiceCollection services,
        ServiceDescriptor serviceDescriptor,
        RegistrationBehavior registerBehavior)
    {
        Guard.Against.Null(services, nameof(services));
        Guard.Against.Null(serviceDescriptor, nameof(serviceDescriptor));
        Guard.Against.EnumOutOfRange(registerBehavior, nameof(registerBehavior));

        switch (registerBehavior)
        {
            case RegistrationBehavior.Replace:
                services.Replace(serviceDescriptor);
                break;

            case RegistrationBehavior.TryAdd:
                services.TryAdd(serviceDescriptor);
                break;

            default:
                services.Add(serviceDescriptor);
                break;
        }
    }
}
