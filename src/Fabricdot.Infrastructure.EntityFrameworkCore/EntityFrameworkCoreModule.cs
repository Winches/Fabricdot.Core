using System;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    [Obsolete("", true)]
    public sealed class EntityFrameworkCoreModule : IModule
    {
        public void Configure(IServiceCollection services)
        {
            services.AddScoped<IEntityChangeTracker, EfEntityChangeTracker>();
            //todo:how to deal with multiple context
        }
    }
}