using System.Diagnostics.CodeAnalysis;
using Fabricdot.Infrastructure.Tests.Aspects.Interceptors;
using Xunit;

namespace Fabricdot.Infrastructure.Tests.Aspects;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("InterceptorTests")]
public class Interceptor_AnnotateTargetType_Tests : InterceptorTestsBase
{
    [Fact]
    public void InvokeDeclaringMethod_AnnotateTargetType_InterceptMethod()
    {
        ResetInterceptorsState();
        var expected = Calculator.Minus(2, 1);
        var actual = IntegerResultInterceptor.Result;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void InvokeDeclaringMethod_AnnotateTargetMethod_InterceptMethod()
    {
        ResetInterceptorsState();
        Calculator.Multiply(2, 1);
        var actual = LoggingInterceptor.IsLogged;
        Assert.True(actual);
    }

    [Fact]
    public void InvokeDeclaringMethod_AnnotateDisableAspect_DoNothing()
    {
        ResetInterceptorsState();
        Calculator.Divide(2, 1);
        Assert.Equal(default, IntegerResultInterceptor.Result);
        Assert.Equal(default, LoggingInterceptor.IsLogged);
    }
}