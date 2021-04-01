using System;
using Fabricdot.Infrastructure.Core.Tracing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fabricdot.WebApi.Core.Tracing
{
    public static class CorrelationMiddlewareExtensions
    {
        public static void AddCorrelationId(
            this IServiceCollection serviceCollection,
            Action<CorrelationIdOptions> optionsBuilder = null)
        {
            serviceCollection.TryAddScoped<ICorrelationIdAccessor, HttpContextCorrelationIdAccessor>();
            serviceCollection.AddTransient<CorrelationMiddleware>();

            serviceCollection.AddOptions<CorrelationIdOptions>();
            serviceCollection.Configure<CorrelationIdOptions>(options => { optionsBuilder?.Invoke(options); });
        }

        public static void UseCorrelationId(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<CorrelationMiddleware>();
        }
    }
}