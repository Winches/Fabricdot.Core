using System.Runtime.CompilerServices;
using AutoMapper;
using Fabricdot.Core.Randoms;
using Fabricdot.Core.Reflection;
using Fabricdot.Core.Security;
using Fabricdot.Domain.Core.Services;
using Fabricdot.Infrastructure.Core.Data.Filters;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Domain;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using Fabricdot.Infrastructure.Core.Domain.Events;
using Fabricdot.Infrastructure.Core.Domain.Services;
using Fabricdot.Infrastructure.Core.Security;
using Fabricdot.Infrastructure.Core.Tracing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: InternalsVisibleTo("Fabricdot.Infrastructure.Core.Tests")]
namespace Fabricdot.Infrastructure.Core
{
    public sealed class InfrastructureModule : IModule
    {
        public void Configure(IServiceCollection services)
        {
            services
                .AddSingleton<IRandomProvider, DefaultRandomProvider>()
                .AddSingleton<IRandomBuilder, RandomBuilder>()
                .AddTransient<IAuditPropertySetter, AuditPropertySetter>();

            services.TryAddSingleton<ICorrelationIdProvider, DefaultCorrelationIdProvider>();

            services
                .AddSingleton<ICurrentPrincipalAccessor, NullCurrentPrincipalAccessor>()
                .AddTransient<ICurrentUser, CurrentUser>();

            //repository filter
            services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>))
                .AddSingleton<IDataFilter, DataFilter>();

            services.AddSingleton<IIdGenerator, GuidGenerator>()
                .AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

            var assemblies = new TypeFinder().GetAssemblies().ToArray();

            services.AddRepositories(assemblies); //repository
            services.AddDomainServices(assemblies); //domain service
            services.AddDomainEventHandlers(assemblies); //domain event handler

            services.AddMediatR(assemblies) //mediator
                .AddAutoMapper(assemblies); //mapper
        }
    }
}