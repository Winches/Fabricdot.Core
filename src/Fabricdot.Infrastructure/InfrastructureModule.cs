using System;
using Fabricdot.Core.Randoms;
using Fabricdot.Core.Security;
using Fabricdot.Core.UniqueIdentifier;
using Fabricdot.Domain.Events;
using Fabricdot.Infrastructure.Data;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.Infrastructure.Domain;
using Fabricdot.Infrastructure.Domain.Auditing;
using Fabricdot.Infrastructure.Domain.Events;
using Fabricdot.Infrastructure.Security;
using Fabricdot.Infrastructure.Tracing;
using Fabricdot.Infrastructure.UniqueIdentifier;
using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fabricdot.Infrastructure
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

            services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
            services.AddSingleton<IGuidGenerator, CombGuidGenerator>();

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