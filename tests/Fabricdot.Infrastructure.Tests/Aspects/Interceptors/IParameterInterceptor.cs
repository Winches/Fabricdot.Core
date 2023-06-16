using Fabricdot.Core.Aspects;

namespace Fabricdot.Infrastructure.Tests.Aspects.Interceptors;

[Interceptor(Order = 1, Target = typeof(IIntegerParameterInterceptorEnabled))]
public interface IParameterInterceptor : IInterceptor
{
}
