using System;
using Fabricdot.Authorization;
using Fabricdot.PermissionGranting.Domain;
using FluentAssertions;
using Xunit;

namespace Fabricdot.PermissionGranting.Tests.Domain;

public class GrantedPermissionTests
{
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [Theory]
    public void Constructor_GivenInvalidObject_Throw(string @object)
    {
        FluentActions.Invoking(() => new GrantedPermission(Guid.NewGuid(), new GrantSubject("type", "a"), @object))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_GivenInput_Correctly()
    {
        var tenantId = Guid.Empty;
        var subject = new GrantSubject("type", "a");
        const string @object = "object1";
        var grantedPermission = new GrantedPermission(tenantId, Guid.NewGuid(), subject, @object);
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