using Fabricdot.Core.Aspects;

namespace Fabricdot.Infrastructure.Core.Tests.Aspects.Interceptors
{
    [Interceptor(Order = 1, Target = typeof(IIntegerParameterInterceptorEnabled))]
    public interface IParameterInterceptor : IInterceptor
    {
    }
}