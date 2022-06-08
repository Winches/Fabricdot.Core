using System;
using Fabricdot.Core.UniqueIdentifier;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Fabricdot.Identity.Domain.Repositories;
using Fabricdot.Identity.Domain.Stores;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Fabricdot.Identity.Domain.Tests.Stores;

public abstract class UserStoreTestsBase
{
    protected UserStore<IdentityUser, IdentityRole> UserStore { get; }

    protected UserStoreTestsBase()
    {
        var mockUserRepository = new Mock<IUserRepository<IdentityUser>>();
        var mockRoleRepository = new Mock<IRoleRepository<IdentityRole>>();

        var mockGuidGenerator = new Mock<IGuidGenerator>();
        mockGuidGenerator.Setup(x => x.Create()).Returns(() => Guid.NewGuid());

        var mockLookupNormalizer = new Mock<ILookupNormalizer>();
        mockLookupNormalizer.Setup(x => x.NormalizeName(It.IsAny<string>())).Returns<string>(v => v.ToUpperInvariant());
        mockLookupNormalizer.Setup(x => x.NormalizeEmail(It.IsAny<string>())).Returns<string>(v => v.ToUpperInvariant());

        UserStore = new UserStore<IdentityUser, IdentityRole>(
            mockUserRepository.Object,
            mockRoleRepository.Object,
            mockGuidGenerator.Object,
            mockLookupNormalizer.Object);
    }
}