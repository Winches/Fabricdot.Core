using Fabricdot.Infrastructure.Core.Tests.Aspects.Interceptors;

namespace Fabricdot.Infrastructure.Core.Tests.Aspects
{
    public class DerivedCalculator : Calculator, IShouldNotInvokedInterceptorEnabled
    {
        /// <inheritdoc />
        public override int Minus(int left, int right) => base.Minus(left, right);
    }
}