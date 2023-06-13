using Ardalis.GuardClauses;
using Fabricdot.AspNetCore.Testing.Middleware;

namespace Microsoft.AspNetCore.Http;

public static class HttpContextExtensions
{
    public static string? ReadRequestBody(this HttpContext context)
    {
        Guard.Against.Null(context, nameof(context));

        return context.Items[RequestBodyKeeperMiddleware.Key]?.ToString();
    }
}
