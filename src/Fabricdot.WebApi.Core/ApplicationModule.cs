using Fabricdot.Common.Core.Security;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Security;
using Fabricdot.WebApi.Core.Filters;
using Fabricdot.WebApi.Core.Services;
using Fabricdot.WebApi.Core.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Core
{
    public sealed class ApplicationModule : IModule
    {
        public void Configure(IServiceCollection services)
        {
            //filters
            services.AddScoped<ExceptionFilter>()
                .AddScoped<ValidationActionFilter>()
                .AddTransient<IModelStateValidator, ModelStateValidator>();

            //principal
            services.AddHttpContextAccessor()
                .AddScoped<ICurrentPrincipalAccessor, HttpContextCurrentPrincipalAccessor>()
                .AddTransient<ICurrentUser, CurrentUser>();

            services.AddMemoryCache();
        }
    }
}