using Fabricdot.Authorization.Permissions;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        Permission Factory() => new(fixture.Create<PermissionName>(), fixture.Create<string>(), fixture.Create<string>());
        fixture.Customize<Permission>(v => v.FromFactory(Factory));
    }
}
