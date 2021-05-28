using Fabricdot.Core.Aspects;
using Fabricdot.Infrastructure.Core.Tests.Aspects.Interceptors;

namespace Fabricdot.Infrastructure.Core.Tests.Aspects
{
    [ResultInterceptor]
    public interface ICalculator : IIntegerParameterInterceptorEnabled
    {
        int Plus(int left, int right);

        int Minus(int left, int right);

        [LoggingInterceptor]
        int Multiply(int left, int right);

        [DisableAspect]
        int Divide(int left, int right);
    }
}