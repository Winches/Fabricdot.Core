using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Tracing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Tracing;

[Dependency(ServiceLifetime.Transient)]
public class CorrelationMiddleware : IMiddleware
{
    private readonly CorrelationIdOptions _idOptions;
    private readonly ICorrelationIdProvider _correlationIdProvider;

    public CorrelationMiddleware(
        IOptions<CorrelationIdOptions> options,
        ICorrelationIdProvider correlationIdProvider)
    {
        _idOptions = options.Value;
        _correlationIdProvider = correlationIdProvider;
    }

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var correlationId = _correlationIdProvider.Get().ToString();
        var headerKey = _idOptions.HeaderKey;

        context.Request.Headers.Add(headerKey, correlationId);
        context.Response.OnStarting(() =>
        {
            if (_idOptions.IncludeResponse)
                context.Response.Headers.Add(headerKey, correlationId);
            return Task.CompletedTask;
        });

        await next.Invoke(context);
    }
}
