using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fabricdot.Domain.Core.Services;

namespace Fabricdot.Infrastructure.Core.Domain
{
    public static class DomainProvider
    {
        internal static IEnumerable<(Type Contract, Type Implementation)> GetRepository(Assembly assembly)
        {
            var repositoryType = typeof(IRepository<,>);
            return assembly.GetTypes()
                .Select(v => new
                {
                    Type = v,
                    Interfaces = v.GetInterfaces()
                })
                .Where(v => v.Type.IsClass && !v.Type.IsAbstract && v.Interfaces.Any(o =>
                    o.IsGenericType && o.GetGenericTypeDefinition() == repositoryType))
                .Select(v => (v.Interfaces.Single(o => !o.IsGenericType && IsSatisfyConvention(o, v.Type)), v.Type));
        }

        internal static IEnumerable<(Type Contract, Type Implementation)> GetServices(Assembly assembly)
        {
            var domainServiceType = typeof(IDomainService);

            return assembly.GetTypes()
                .Select(v => new
                {
                    Type = v,
                    Interfaces = v.GetInterfaces()
                })
                .Where(v => v.Type.IsClass && !v.Type.IsAbstract && v.Interfaces.Contains(domainServiceType))
                .Select(v => (v.Interfaces.Single(o => !o.IsGenericType && IsSatisfyConvention(o, v.Type)), v.Type));
        }

        internal static bool IsSatisfyConvention(Type destination, Type source)
        {
            var name = source.IsInterface ? source.Name.Trim('I') : source.Name;
            return destination.Name.EndsWith(name);
        }
    }
}