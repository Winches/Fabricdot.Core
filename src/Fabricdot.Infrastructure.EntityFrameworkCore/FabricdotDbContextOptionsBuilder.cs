using System.Reflection;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fabricdot.Infrastructure.EntityFrameworkCore;

public class FabricdotDbContextOptionsBuilder
{
    public IServiceCollection Services { get; protected set; }

    public Type ContextType { get; protected set; }

    public FabricdotDbContextOptionsBuilder(
        IServiceCollection services,
        Type contextType)
    {
        Services = services;
        ContextType = contextType;
    }

    public FabricdotDbContextOptionsBuilder AddDefaultRepositories()
    {
        GetEntityTypes(ContextType).ForEach(v => AddDefaultRepository(v));
        return this;
    }

    public FabricdotDbContextOptionsBuilder AddDefaultRepository<TEntity>() where TEntity : IAggregateRoot, new()
    {
        return AddDefaultRepository(typeof(TEntity));
    }

    public FabricdotDbContextOptionsBuilder AddDefaultRepository(Type entityType)
    {
        if (!entityType.IsAssignableTo(typeof(IAggregateRoot)))
            throw new ArgumentException($"The {entityType.PrettyPrint()} is not an aggregate root.");
        if (!entityType.IsAssignableToGenericType(typeof(IEntity<>)))
            throw new ArgumentException($"The {entityType.PrettyPrint()} is not an entity.");

        var keyType = entityType.GetInterfaces().First(v => v.IsGenericType && v.IsAssignableToGenericType(typeof(IEntity<>))).GenericTypeArguments[0];
        var serviceType = typeof(IRepository<,>).MakeGenericType(entityType, keyType);
        if (!Services.Any(v => v.ServiceType.IsAssignableTo(serviceType)))
        {
            var implementationType = typeof(EfRepository<,,>).MakeGenericType(ContextType, entityType, keyType);
            Services.TryAddScoped(serviceType, implementationType);
        }

        return this;
    }

    protected static IEnumerable<Type> GetEntityTypes(Type contextType)
    {
        return contextType.GetTypeInfo()
                           .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                           .Where(v => v.PropertyType.IsAssignableToGenericType(typeof(DbSet<>)))
                           .Select(v => v.PropertyType.GenericTypeArguments[0])
                           .Where(v => v.IsAssignableTo(typeof(IAggregateRoot)) && v.IsAssignableToGenericType(typeof(IEntity<>)));
    }
}
