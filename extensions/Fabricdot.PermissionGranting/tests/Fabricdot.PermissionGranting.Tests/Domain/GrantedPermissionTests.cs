using Fabricdot.Authorization;
using Fabricdot.PermissionGranting.Domain;

namespace Fabricdot.PermissionGranting.Tests.Domain;

public class GrantedPermissionTests : TestBase
{
    [Fact]
    public void Constructor_GivenInvalidObject_Throw()
    {
        var sut = typeof(GrantedPermission).GetConstructors();

        Create<GuardClauseAssertion>().Verify(sut);
    }

    [AutoData]
    [Theory]
    public void Constructor_GivenInput_Correctly(
        Guid tenantId,
        Guid id,
        GrantSubject subject,
        string @object)
    {
        var grantedPermission = new GrantedPermission(tenantId, id, subject, @object);
        var expected = new
        {
            TenantId = tenantId,
            GrantType = subject.Type,
            Subject = subject.Value,
            Object = @object
        };

        grantedPermission.Should().BeEquivalentTo(expected);
    }
}