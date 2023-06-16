using Ardalis.GuardClauses;
using Fabricdot.PermissionGranting.Domain;
using Fabricdot.PermissionGranting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddPermissionGrantingStore<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext, IPermissionGrantingDbContext
    {
        Guard.Against.Null(services, nameof(services));

        services.AddScoped<IGrantedPermissionRepository, GrantedPermissionRepository<TDbContext>>();
    }
}
