using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Fabricdot.Infrastructure.Core.Tests.Aspects.Interceptors;
using Xunit;

namespace Fabricdot.Infrastructure.Core.Tests.Aspects
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Collection("InterceptorTests")]
    public class Interceptor_SpecificTargetType_Tests : InterceptorTestsBase
    {
        [Fact]
        public void InvokeDeclaringMethod_InheritTargetTypeDirectly_InterceptMethod()
        {
            ResetInterceptorsState();
            var expected = new[]
            {
                1, 2
            };
            Calculator.Plus(expected.First(), expected.Last());
            var actual = IntegerParameterInterceptor.Parameters;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InvokeInheritedMethod_InheritTargetTypeIndirectly_InterceptMethod()
        {
            ResetInterceptorsState();
            var expected = new[]
            {
                1, 2
            };
            // Interceptor with specific marker type should not be invoke.
            DerivedCalculator.Plus(expected.First(), expected.Last());
            var actual = IntegerParameterInterceptor.Parameters;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InvokeOverridingMethod_InheritTargetTypeIndirectly_InterceptMethod()
        {
            ResetInterceptorsState();
            var expected = new[]
            {
                1, 2
            };
            // Interceptor with specific marker type should not be invoke.
            DerivedCalculator.Minus(expected.First(), expected.Last());
            var actual = IntegerParameterInterceptor.Parameters;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InvokeDeclaringMethod_AnnotateDisableAspect_DoNothing()
        {
            ResetInterceptorsState();
            Calculator.Divide(2, 1);
            Assert.Equal(default, IntegerParameterInterceptor.Parameters);
        }
    }
}