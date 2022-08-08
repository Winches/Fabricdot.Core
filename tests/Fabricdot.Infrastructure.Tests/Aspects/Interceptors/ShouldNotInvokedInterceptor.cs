using Fabricdot.Core.Aspects;
using Fabricdot.Core.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Aspects.Interceptors;

[ServiceContract(typeof(ShouldNotInvokedInterceptor))]
[Interceptor(Target = typeof(IShouldNotInvokedInterceptorEnabled))]
internal class ShouldNotInvokedInterceptor : IInterceptor, ITransientDependency
{
    /// <inheritdoc />
    public Task InvokeAsync(IInvocationContext invocationContext) => throw new System.NotImplementedException();
}