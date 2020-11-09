using System;
using System.Linq;
using AutoMapper;
using Fabricdot.Common.Core.Logging;
using Fabricdot.Common.Core.Randoms;
using Fabricdot.Domain.Core.Services;
using Fabricdot.Infrastructure.Core.Data.Filters;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using Fabricdot.Infrastructure.Core.Domain.Events;
using Fabricdot.Infrastructure.Core.Domain.Services;
using Fabricdot.Infrastructure.Core.Logging;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Core
{
    public sealed class InfrastructureModule : IModule
    {
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<IRandomNumberGenerator, RandomNumberGenerator>()
                .AddSingleton<IRandomBuilder, RandomBuilder>()
                .AddTransient<IAuditPropertySetter, AuditPropertySetter>()
                .AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

            //repository filter
            services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>))
                .AddSingleton<IDataFilter, DataFilter>();

            services.AddSingleton<IIdGenerator, GuidGenerator>()
                .AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(v => !v.FullName.StartsWith("System") && !v.FullName.StartsWith("Microsoft"))
                .ToArray(); //todo:encapsulate

            services.AddMediatR(assemblies) //mediator
                .AddAutoMapper(assemblies); //mapper
        }
    }
}