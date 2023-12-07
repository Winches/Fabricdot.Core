using Fabricdot.Core.Aspects;

namespace Fabricdot.Infrastructure.Aspects;

public class InterceptorCollection : HashSet<InterceptorDescriptor>
{
    public void Add(Type interceptorType)
    {
        ArgumentNullException.ThrowIfNull(interceptorType);

        var descriptor = InterceptorDescriptor.Create(interceptorType);
        if (!Add(descriptor))
            throw new ArgumentException($"Interceptor of {interceptorType.Name} already registered.");
    }

    public void Add<TInterceptor>() where TInterceptor : IInterceptor => Add(typeof(TInterceptor));
}
