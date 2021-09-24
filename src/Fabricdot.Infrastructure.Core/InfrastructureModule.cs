using System;
using Fabricdot.Core.Randoms;
using Fabricdot.Core.Security;
using Fabricdot.Domain.Core.Services;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.Core.Data.Filters;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Domain;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using Fabricdot.Infrastructure.Core.Domain.Events;
using Fabricdot.Infrastructure.Core.Domain.Services;
using Fabricdot.Infrastructure.Core.Security;
using Fabricdot.Infrastructure.Core.Tracing;
using Fabricdot.Infrastructure.Core.Uow;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
                .AddSingleton<ICurrentPrincipalAccessor, DefaultCurrentPrincipalAccessor>()
                .AddTransient<ICurrentUser, CurrentUser>();

            //repository filter
            services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>))
                .AddSingleton<IDataFilter, DataFilter>();
            services.AddTransient<IConnectionStringResolver, DefaultConnectionStringResolver>();

            services.AddSingleton<IIdGenerator, GuidGenerator>()
                .AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

            //var assemblies = new TypeFinder().GetAssemblies().ToArray();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            services.AddRepositories(assemblies); //repository
            services.AddDomainServices(assemblies); //domain service
            services.AddDomainEventHandlers(assemblies); //domain event handler

            services.AddMediatR(assemblies) //mediator
                .AddAutoMapper(assemblies); //mapper

            //unit-of-work
            services.AddTransient<IUnitOfWorkFacade, UnitOfWorkFacade>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IAmbientUnitOfWork, AmbientUnitOfWork>();
            services.AddSingleton<IUnitOfWorkManager, UnitOfWorkManager>();
            services.AddSingleton<IUnitOfWorkTransactionBehaviourProvider, DefaultUnitOfWorkTransactionBehaviourProvider>();
            services.AddTransient<UnitOfWorkInterceptor>();
        }
    }
}