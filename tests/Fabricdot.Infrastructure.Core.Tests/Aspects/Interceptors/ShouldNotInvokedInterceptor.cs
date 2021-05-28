using System.Threading.Tasks;
using Fabricdot.Core.Aspects;

namespace Fabricdot.Infrastructure.Core.Tests.Aspects.Interceptors
{
    [Interceptor(Target = typeof(IShouldNotInvokedInterceptorEnabled))]
    internal class ShouldNotInvokedInterceptor : IInterceptor
    {
        /// <inheritdoc />
        public Task InvokeAsync(IInvocationContext invocationContext) => throw new System.NotImplementedException();
    }
}