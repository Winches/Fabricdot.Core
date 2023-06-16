using Fabricdot.AspNetCore.Testing;
using Fabricdot.WebApi.Tracing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Tests.Tracing;

public class CorrelationMiddlewareTests : WebApplicationTestBase<WebApiTestModule>
{
    public CorrelationMiddlewareTests(TestWebApplicationFactory<WebApiTestModule> webAppFactory) : base(webAppFactory)
    {
    }

    [Fact]
    public async Task CorrelationMiddleware_ExcludeResponse_SetRequestHeaderOnly()
    {
        var options = ConfigureOptions(v => v.IncludeResponse = false);
        var httpContext = await Server.SendAsync(c =>
        {
            c.Request.Method = HttpMethods.Get;
            c.Request.Path = "/";
        });

        httpContext.Request.Headers.Should().Contain(v => v.Key == options.HeaderKey && !v.Value.IsNullOrEmpty());
        httpContext.Response.Headers.Should().NotContain(v => v.Key == options.HeaderKey);
    }

    [Fact]
    public async Task CorrelationMiddleware_IncludeResponse_SetResponseHeader()
    {
        var options = ConfigureOptions(v => v.IncludeResponse = true);
        var httpContext = await Server.SendAsync(c =>
        {
            c.Request.Method = HttpMethods.Get;
            c.Request.Path = "/";
        });
        var expected = httpContext.Request.Headers.GetOrDefault(options.HeaderKey);
        var actual = httpContext.Response.Headers.GetOrDefault(options.HeaderKey);

        actual.Should().BeEquivalentTo(expected);
    }

    private CorrelationIdOptions ConfigureOptions(Action<CorrelationIdOptions> action)
    {
        var options = ServiceProvider.GetRequiredService<IOptions<CorrelationIdOptions>>().Value;
        action.Invoke(options);
        return options;
    }
}
