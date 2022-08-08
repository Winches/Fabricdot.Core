using Fabricdot.Infrastructure.Tests.Aspects.Interceptors;

namespace Fabricdot.Infrastructure.Tests.Aspects;

[Collection("InterceptorTests")]
public class Interceptor_SpecificTargetType_Tests : InterceptorTestsBase
{
    [Fact]
    public void InvokeDeclaringMethod_InheritTargetTypeDirectly_InterceptMethod()
    {
        ResetInterceptorsState();
        var expected = Fixture.CreateMany<int>(2).ToArray();
        Calculator.Plus(expected[0], expected[^1]);

        IntegerParameterInterceptor.Parameters.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void InvokeInheritedMethod_InheritTargetTypeIndirectly_InterceptMethod()
    {
        ResetInterceptorsState();
        var expected = Fixture.CreateMany<int>(2).ToArray();
        // Interceptor with specific marker type should not be invoke.
        DerivedCalculator.Plus(expected[0], expected[^1]);

        IntegerParameterInterceptor.Parameters.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void InvokeOverridingMethod_InheritTargetTypeIndirectly_InterceptMethod()
    {
        ResetInterceptorsState();
        var expected = Fixture.CreateMany<int>(2).ToArray();
        // Interceptor with specific marker type should not be invoke.
        DerivedCalculator.Minus(expected[0], expected[^1]);

        IntegerParameterInterceptor.Parameters.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void InvokeDeclaringMethod_AnnotateDisableAspect_DoNothing()
    {
        ResetInterceptorsState();
        Calculator.Divide(Create<int>(), Create<int>());

        IntegerParameterInterceptor.Parameters.Should().BeNull();
    }
}