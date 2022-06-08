using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fabricdot.WebApi.Tracing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Fabricdot.WebApi.Tests.Tracing;

public class CorrelationMiddlewareTests : AspNetCoreTestsBase<Startup>
{
    [Fact]
    public async Task CorrelationMiddleware_ExcludeResponse_SetRequestHeaderOnly()
    {
        var options = ConfigureOptions(v => v.IncludeResponse = false);
        IHeaderDictionary requestHeader = null;
        ServiceProvider.GetRequiredService<ActionMiddlewareProvider>()
            .ExecutingAction = context => CaptureRequestHeadersAsync(context, ref requestHeader);

        var response = await HttpClient.GetAsync("/");
        var requestCorrelationId = GetCorrelationId(requestHeader, options.HeaderKey);
        var responseCorrelationId = GetCorrelationId(response.Headers, options.HeaderKey);

        Assert.NotNull(requestCorrelationId);
        Assert.Null(responseCorrelationId);
    }

    [Fact]
    public async Task CorrelationMiddleware_IncludeResponse_SetResponseHeader()
    {
        var options = ConfigureOptions(v => v.IncludeResponse = true);
        IHeaderDictionary requestHeader = null;
        ServiceProvider.GetRequiredService<ActionMiddlewareProvider>()
            .ExecutingAction = context => CaptureRequestHeadersAsync(context, ref requestHeader);

        var response = await HttpClient.GetAsync("/");
        var requestCorrelationId = GetCorrelationId(requestHeader, options.HeaderKey);
        var responseCorrelationId = GetCorrelationId(response.Headers, options.HeaderKey);

        Assert.NotNull(requestCorrelationId);
        Assert.Equal(requestCorrelationId, responseCorrelationId);
    }

    private static Task CaptureRequestHeadersAsync(HttpContext context, ref IHeaderDictionary requestHeader)
    {
        requestHeader = context.Request.Headers;
        return Task.CompletedTask;
    }

    private static string GetCorrelationId(IHeaderDictionary headers, string name)
    {
        if (headers.TryGetValue(name, out var values))
            return values[0];

        return null;
    }

    private static string GetCorrelationId(HttpHeaders headers, string name)
    {
        if (headers.TryGetValues(name, out var values))
            return values.First();
        return null;
    }

    private CorrelationIdOptions ConfigureOptions(Action<CorrelationIdOptions> action = null)
    {
        var options = ServiceProvider.GetRequiredService<IOptions<CorrelationIdOptions>>().Value;
        action?.Invoke(options);
        return options;
    }
}