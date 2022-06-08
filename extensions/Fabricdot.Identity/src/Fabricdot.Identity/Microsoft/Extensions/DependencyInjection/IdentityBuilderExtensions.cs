using Ardalis.GuardClauses;
using Fabricdot.Domain.Services;
using Fabricdot.Identity;
using Fabricdot.Identity.Domain.Repositories;
using Fabricdot.Identity.Domain.Stores;
using Fabricdot.Identity.Infrastructure.Data.Repositories;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class IdentityBuilderExtensions
{
    public static IdentityBuilder AddRepositories<TDbContext>(this IdentityBuilder builder) where TDbContext : DbContextBase
    {
        Guard.Against.Null(builder, nameof(builder));

        return builder.AddUserRepository<TDbContext>()
                      .AddRoleRepository<TDbContext>();
    }

    public static IdentityBuilder AddUserRepository<TDbContext>(this IdentityBuilder builder) where TDbContext : DbContextBase
    {
        Guard.Against.Null(builder, nameof(builder));

        var (services, userType, roleType) = (builder.Services, builder.UserType, builder.RoleType);
        var repositoryImplType = typeof(UserRepository<,>).MakeGenericType(typeof(TDbContext), userType);
        services.TryAddTransient(
            typeof(IUserRepository<>).MakeGenericType(userType),
            repositoryImplType);
        services.TryAddTransient(
            typeof(IReadOnlyRepository<>).MakeGenericType(userType),
            repositoryImplType);
        services.AddScoped(
            typeof(IUserStore<>).MakeGenericType(userType),
            typeof(UserStore<,>).MakeGenericType(userType, roleType));

        return builder;
    }

    public static IdentityBuilder AddRoleRepository<TDbContext>(this IdentityBuilder builder) where TDbContext : DbContextBase
    {
        Guard.Against.Null(builder, nameof(builder));

        var (services, roleType) = (builder.Services, builder.RoleType);
        var repositoryImplType = typeof(RoleRepository<,>).MakeGenericType(typeof(TDbContext), roleType);
        services.TryAddTransient(
            typeof(IRoleRepository<>).MakeGenericType(roleType),
            repositoryImplType);
        services.TryAddTransient(
            typeof(IReadOnlyRepository<>).MakeGenericType(roleType),
            repositoryImplType);

        services.AddScoped(
            typeof(IRoleStore<>).MakeGenericType(roleType),
            typeof(RoleStore<>).MakeGenericType(roleType));

        return builder;
    }

    public static IdentityBuilder AddDefaultClaimsPrincipalFactory(this IdentityBuilder builder)
    {
        Guard.Against.Null(builder, nameof(builder));

        var (services, userType, roleType) = (builder.Services, builder.UserType, builder.RoleType);
        services.AddScoped(
            typeof(IUserClaimsPrincipalFactory<>).MakeGenericType(userType),
            typeof(DefaultUserClaimsPrincipalFactory<,>).MakeGenericType(userType, roleType));

        return builder;
    }
}