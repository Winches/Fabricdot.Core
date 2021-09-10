using System;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Core.Uow
{
    public static class UnitOfWorkMiddlewareExtensions
    {
        public static IServiceCollection AddUnitOfWork(
            this IServiceCollection services,
            Action<HttpUnitOfWorkOptions> optionsBuilder = null)
        {
            Guard.Against.Null(services, nameof(services));

            services.AddTransient<UnitOfWorkMiddleware>();
            services.AddOptions<HttpUnitOfWorkOptions>();
            services.Configure<HttpUnitOfWorkOptions>(v => optionsBuilder?.Invoke(v));

            return services;
        }

        public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder applicationBuilder)
        {
            Guard.Against.Null(applicationBuilder, nameof(applicationBuilder));

            applicationBuilder.UseMiddleware<UnitOfWorkMiddleware>();
            return applicationBuilder;
        }
    }
}