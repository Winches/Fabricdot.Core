using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;

namespace Fabricdot.MultiTenancy.AspNetCore.Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder applicationBuilder)
        {
            Guard.Against.Null(applicationBuilder, nameof(applicationBuilder));

            applicationBuilder.UseMiddleware<MultiTenancyMiddleware>();
            return applicationBuilder;
        }
    }
}