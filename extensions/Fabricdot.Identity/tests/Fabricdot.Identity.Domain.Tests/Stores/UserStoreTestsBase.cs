using Fabricdot.Core.UniqueIdentifier;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Fabricdot.Identity.Domain.Repositories;
using Fabricdot.Identity.Domain.Stores;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Fabricdot.Identity.Domain.Tests.Stores;

public abstract class UserStoreTestsBase : TestFor<UserStore<IdentityUser, IdentityRole>>
{
    protected UserStoreTestsBase()
    {
        var mockUserRepository = InjectMock<IUserRepository<IdentityUser>>();
        var mockRoleRepository = InjectMock<IRoleRepository<IdentityRole>>();
        var mockGuidGenerator = InjectMock<IGuidGenerator>();
        var mockLookupNormalizer = InjectMock<ILookupNormalizer>();

        mockGuidGenerator.Setup(x => x.Create()).Returns(() => Guid.NewGuid());
        mockLookupNormalizer.Setup(x => x.NormalizeName(It.IsAny<string>())).Returns<string>(v => v.ToUpperInvariant());
        mockLookupNormalizer.Setup(x => x.NormalizeEmail(It.IsAny<string>())).Returns<string>(v => v.ToUpperInvariant());
    }
}