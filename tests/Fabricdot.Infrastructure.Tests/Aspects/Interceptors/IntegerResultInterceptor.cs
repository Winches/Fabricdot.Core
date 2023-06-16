using Fabricdot.Core.Aspects;
using Fabricdot.Core.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Aspects.Interceptors;

[Interceptor(Order = 2)]
[ResultInterceptor]
public class IntegerResultInterceptor : IInterceptor, ITransientDependency
{
    public static int Result { get; set; }

    /// <inheritdoc />
    public async Task InvokeAsync(IInvocationContext invocationContext)
    {
        await invocationContext.ProceedAsync();
        Result = invocationContext.ReturnValue is int value ? value : 0;
    }
}
