using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Tests.Aspects.Interceptors;

namespace Fabricdot.Infrastructure.Tests.Aspects
{
    [ServiceContract(typeof(DerivedCalculator))]
    public class DerivedCalculator : Calculator, IShouldNotInvokedInterceptorEnabled
    {
        /// <inheritdoc />
        public override int Minus(int left, int right) => base.Minus(left, right);
    }
}