using Fabricdot.Core.Security;
using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Fabricdot.WebApi.ExceptionHanding;
using Fabricdot.WebApi.Filters;
using Fabricdot.WebApi.Securirty;
using Fabricdot.WebApi.Tracing;
using Fabricdot.WebApi.Uow;
using Fabricdot.WebApi.Validation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi
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
            services.AddSingleton<IPrincipalAccessor, HttpContextPrincipalAccessor>();

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