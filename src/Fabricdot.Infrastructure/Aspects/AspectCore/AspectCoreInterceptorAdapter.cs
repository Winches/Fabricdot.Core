using AspectCore.DynamicProxy;
using Fabricdot.Core.Aspects;
using IInterceptor = Fabricdot.Core.Aspects.IInterceptor;

namespace Fabricdot.Infrastructure.Aspects.AspectCore;

[DisableAspect]
public sealed class AspectCoreInterceptorAdapter<TInterceptor> : AbstractInterceptor
    where TInterceptor : IInterceptor
{
    private readonly TInterceptor _interceptor;

    public AspectCoreInterceptorAdapter(TInterceptor interceptor)
    {
        _interceptor = interceptor;
    }

    /// <inheritdoc />
    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        var invocationContext = new AspectCoreInvocationContextAdapter(context, next);
        await _interceptor.InvokeAsync(invocationContext);
    }
}