using Fabricdot.Identity.Domain.Repositories;
using Fabricdot.Identity.Tests.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Identity.Tests.Domain.Stores
{
    public abstract class UserStoreTestBase : IdentityTestBase
    {
        protected IUserRepository<User> UserRepository { get; }

        protected ILookupNormalizer LookupNormalizer { get; }

        protected IUserStore<User> UserStore { get; }

        protected UserStoreTestBase()
        {
            UserRepository = ScopeServiceProvider.GetRequiredService<IUserRepository<User>>();
            LookupNormalizer = ScopeServiceProvider.GetRequiredService<ILookupNormalizer>();
            UserStore = ScopeServiceProvider.GetRequiredService<IUserStore<User>>();
        }
    }
}