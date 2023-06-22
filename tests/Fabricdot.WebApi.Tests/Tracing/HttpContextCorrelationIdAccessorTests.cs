using Fabricdot.Infrastructure.Tracing;
using Fabricdot.Testing;
using Fabricdot.WebApi.Tracing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Tests.Tracing;

public class HttpContextCorrelationIdAccessorTests : TestFor<HttpContextCorrelationIdAccessor>
{
    [Fact]
    public void CorrelationId_WhenHttpContextIsNull_ReturnNull()
    {
        var mockContextAccessor = InjectMock<IHttpContextAccessor>();
        mockContextAccessor.SetupProperty(v => v.HttpContext);

        mockContextAccessor.Object.HttpContext.Should().BeNull();
        Sut.CorrelationId.Should().BeNull();
    }

    [AutoMockData]
    [Theory]
    public void CorrelationId_WhenHeaderNotFound_ReturnNull(HttpContext httpContext)
    {
        var mockContextAccessor = InjectMock<IHttpContextAccessor>();
        mockContextAccessor.SetupProperty(v => v.HttpContext, httpContext);

        mockContextAccessor.Object.HttpContext.Should().NotBeNull();
        Sut.CorrelationId.Should().BeNull();
    }

    [AutoMockData]
    [Theory]
    public void CorrelationId_WhenHeaderExists_ReturnNull(
        [Frozen] IOptions<CorrelationIdOptions> options,
        DefaultHttpContext httpContext,
        CorrelationId expected)
    {
        var mockContextAccessor = InjectMock<IHttpContextAccessor>()!;
        httpContext.Request.Headers.TryAdd(options.Value.HeaderKey, expected.ToString());
        mockContextAccessor.SetupProperty(v => v.HttpContext, httpContext);

        mockContextAccessor.Object.HttpContext.Should().NotBeNull();
        Sut.CorrelationId.Should().Be(expected);
    }
}
