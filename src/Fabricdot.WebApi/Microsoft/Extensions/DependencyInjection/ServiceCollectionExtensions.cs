using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Fabricdot.Core.Boot;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IApplicationBuilder GetApplicationBuilder(this IServiceProvider serviceProvider)
        {
            Guard.Against.Null(serviceProvider, nameof(serviceProvider));

            var app = serviceProvider.GetBuilderProperties()
                                     .GetOrDefault(BootstrapperBuilderProperties.ApplicationBuilder);
            return (IApplicationBuilder?)app ?? throw new InvalidOperationException("ApplicationBuilder not existed.");
        }
    }
}