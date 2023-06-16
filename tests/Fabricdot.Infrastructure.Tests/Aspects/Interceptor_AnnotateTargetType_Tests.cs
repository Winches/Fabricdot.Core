using Fabricdot.Infrastructure.Tests.Aspects.Interceptors;

namespace Fabricdot.Infrastructure.Tests.Aspects;

[Collection("InterceptorTests")]
public class Interceptor_AnnotateTargetType_Tests : InterceptorTestsBase
{
    [Fact]
    public void InvokeDeclaringMethod_AnnotateTargetType_InterceptMethod()
    {
        ResetInterceptorsState();
        var expected = Calculator.Minus(Create<int>(), Create<int>());

        IntegerResultInterceptor.Result.Should().Be(expected);
    }

    [Fact]
    public void InvokeDeclaringMethod_AnnotateTargetMethod_InterceptMethod()
    {
        ResetInterceptorsState();
        Calculator.Multiply(Create<int>(), Create<int>());

        LoggingInterceptor.IsLogged.Should().BeTrue();
    }

    [Fact]
    public void InvokeDeclaringMethod_AnnotateDisableAspect_DoNothing()
    {
        ResetInterceptorsState();
        Calculator.Divide(Create<int>(), Create<int>());

        IntegerResultInterceptor.Result.Should().Be(default);
        LoggingInterceptor.IsLogged.Should().Be(default);
    }
}
