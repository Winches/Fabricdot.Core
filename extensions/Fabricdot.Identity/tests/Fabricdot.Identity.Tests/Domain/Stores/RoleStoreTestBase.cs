using Fabricdot.Identity.Domain.Repositories;
using Fabricdot.Identity.Tests.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Identity.Tests.Domain.Stores;

public abstract class RoleStoreTestBase : IdentityTestBase
{
    protected IRoleRepository<Role> RoleRepository { get; }

    protected ILookupNormalizer LookupNormalizer { get; }

    protected IRoleStore<Role> RoleStore { get; }

    protected RoleStoreTestBase()
    {
        RoleRepository = ServiceProvider.GetRequiredService<IRoleRepository<Role>>();
        LookupNormalizer = ServiceProvider.GetRequiredService<ILookupNormalizer>();
        RoleStore = ServiceProvider.GetRequiredService<IRoleStore<Role>>();
    }
}