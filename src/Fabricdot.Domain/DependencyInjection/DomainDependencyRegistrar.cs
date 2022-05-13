using System;
using System.Collections.Generic;
using System.Linq;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Domain.Events;
using Fabricdot.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Domain.DependencyInjection
{
    public class DomainDependencyRegistrar : DefaultDependencyRegistrar
    {
        protected static readonly Type DomainEventHanlderType = typeof(IDomainEventHandler<>);

        protected static readonly Type DomainServiceType = typeof(IDomainService);

        protected static readonly Type RepositoryType = typeof(IRepository);

        protected override bool CanRegister(Type typeToRegister)
        {
            return (typeToRegister.IsAssignableTo(DomainServiceType)
                    || typeToRegister.IsAssignableTo(RepositoryType)
                    || typeToRegister.IsAssignableToGenericType(DomainEventHanlderType))
                   && base.CanRegister(typeToRegister);
        }

        protected override ServiceLifetime? GetDefaultLifetime(Type type) => ServiceLifetime.Transient;

        protected override ICollection<Type> GetServiceTypes(Type implementationType)
        {
            // Domain event handler
            if (implementationType.IsAssignableToGenericType(DomainEventHanlderType))
            {
                return implementationType.GetInterfaces()
                                         .Where(v => v.IsAssignableToGenericType(DomainEventHanlderType))
                                         .ToArray();
            }

            // Domain service
            var types = base.GetServiceTypes(implementationType);
            if (implementationType.IsAssignableTo(DomainServiceType))
            {
                return types.Where(v => v.IsInterface && v != DomainServiceType).ToArray();
            }
            // Repository
            if (implementationType.IsAssignableTo(RepositoryType))
            {
                return types.Where(v => v != RepositoryType).ToArray();
            }

            throw new InvalidOperationException("Invalid domain type.");
        }
    }
}