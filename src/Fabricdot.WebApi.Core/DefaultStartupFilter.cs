using System;
using Fabricdot.WebApi.Core.Tracing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Fabricdot.WebApi.Core
{
    public class DefaultStartupFilter : IStartupFilter
    {
        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                builder.UseCorrelationId();
                next(builder);
            };
        }
    }
}