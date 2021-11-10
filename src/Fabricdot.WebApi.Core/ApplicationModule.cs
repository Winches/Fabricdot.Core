using Fabricdot.Core.Security;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;
using Fabricdot.WebApi.Core.ExceptionHanding;
using Fabricdot.WebApi.Core.Filters;
using Fabricdot.WebApi.Core.Services;
using Fabricdot.WebApi.Core.Tracing;
using Fabricdot.WebApi.Core.Uow;
using Fabricdot.WebApi.Core.Validation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Core
{
    public sealed class ApplicationModule : IModule
    {
        public void Configure(IServiceCollection services)
        {
            //filters
            services.AddScoped<ExceptionHandlingFilter>()
                .AddScoped<ValidationActionFilter>()
                .AddScoped<UnitOfWorkActionFilter>()
                .AddScoped<ResultFilter>()
                .AddTransient<IModelStateValidator, ModelStateValidator>();

            //principal
            services.AddHttpContextAccessor();
            services.AddSingleton<ICurrentPrincipalAccessor, HttpContextCurrentPrincipalAccessor>();

            //tracing
            services.AddCorrelationId();

            services.AddMemoryCache();

            //unit-of-work
            services.AddSingleton<IUnitOfWorkTransactionBehaviourProvider, HttpUnitOfWorkTransactionBehaviourProvider>();
            services.AddUnitOfWork();

            services.AddTransient<IStartupFilter, DefaultStartupFilter>();
        }
    }
}