using Fabricdot.Infrastructure.Tests.Aspects.Interceptors;

namespace Fabricdot.Infrastructure.Tests.Aspects
{
    public class DerivedCalculator : Calculator, IShouldNotInvokedInterceptorEnabled
    {
        /// <inheritdoc />
        public override int Minus(int left, int right) => base.Minus(left, right);
    }
}