using Fabricdot.Identity.Domain.Repositories;
using Fabricdot.Identity.Tests.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Identity.Tests.Domain.Stores;

public abstract class UserStoreTestBase : IdentityTestBase
{
    protected IUserRepository<User> UserRepository { get; }

    protected ILookupNormalizer LookupNormalizer { get; }

    protected IUserStore<User> UserStore { get; }

    protected UserStoreTestBase()
    {
        UserRepository = ServiceProvider.GetRequiredService<IUserRepository<User>>();
        LookupNormalizer = ServiceProvider.GetRequiredService<ILookupNormalizer>();
        UserStore = ServiceProvider.GetRequiredService<IUserStore<User>>();
    }
}