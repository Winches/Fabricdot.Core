using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;

namespace Fabricdot.WebApi.Tracing
{
    public static class CorrelationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder applicationBuilder)
        {
            Guard.Against.Null(applicationBuilder, nameof(applicationBuilder));

            applicationBuilder.UseMiddleware<CorrelationMiddleware>();
            return applicationBuilder;
        }
    }
}