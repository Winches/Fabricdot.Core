using System;
using System.Linq;
using Fabricdot.Core.Aspects;
using JetBrains.Annotations;

namespace Fabricdot.Infrastructure.Aspects;

public class InterceptorDescriptor
{
    public Type InterceptorType { get; }

    public Type? TargetType { get; }

    public Type? BindingType { get; }

    public int Order { get; }

    public InterceptorDescriptor(
        [NotNull] Type interceptorType,
        int order,
        [CanBeNull] Type? targetType,
        [CanBeNull] Type? bindingType)
    {
        if (interceptorType == null)
            throw new ArgumentNullException(nameof(interceptorType));
        if (!interceptorType.IsAssignableTo(typeof(IInterceptor)))
        {
            throw new ArgumentException(
                "Interceptor must derived from IInterceptorMetadata", nameof(interceptorType));
        }

        if (targetType is null && bindingType is null)
            throw new ArgumentException("Target type and binding type both is null.");

        InterceptorType = interceptorType;
        Order = order;
        TargetType = targetType;
        BindingType = bindingType;
    }

    public static InterceptorDescriptor Create([NotNull] Type interceptorType)
    {
        var attributes = interceptorType.GetCustomAttributes(true).ToArray();
        var interceptorAttribute = attributes.OfType<InterceptorAttribute>().SingleOrDefault();
        var bindingAttribute =
            attributes.FirstOrDefault(v => v.GetType().IsDefined(typeof(InterceptorBindingAttribute), true));

        var order = interceptorAttribute?.Order ?? 0;
        return new InterceptorDescriptor(
            interceptorType, order, interceptorAttribute?.Target, bindingAttribute?.GetType());
    }

    /// <inheritdoc />
    public override int GetHashCode() => InterceptorType.GetHashCode();

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is InterceptorDescriptor descriptor && InterceptorType == descriptor.InterceptorType;
    }
}