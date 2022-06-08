using System;
using Fabricdot.Authorization.Permissions;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionGroupTests
{
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [Theory]
    public void Constructor_GivenInvalidName_Throw(string name)
    {
        FluentActions.Invoking(() => new PermissionGroup(name, "group1"))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [Theory]
    public void DisplyName_GivenInvalidInput_Throw(string dispalyName)
    {
        FluentActions.Invoking(() => new PermissionGroup("name", dispalyName))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [Fact]
    public void AddPermission_GivenInput_Correctly()
    {
        var expected = new
        {
            Name = new PermissionName("Object1.Create"),
            DisplayName = "create object1",
            Description = "description"
        };

        var group = new PermissionGroup("name", "dispaly name");
        var permission = group.AddPermission(
            expected.Name,
            expected.DisplayName,
            expected.Description);

        permission.Should().BeEquivalentTo(expected);
        group.Permissions.Should().Contain(permission);
    }

    [Fact]
    public void AddPermission_GivenDuplicateValue_Throw()
    {
        var group = new PermissionGroup("name", "dispaly name");
        const string name = "Object1.Create";
        group.AddPermission(name, "create object1");

        FluentActions.Invoking(() => group.AddPermission(name, "create object1"))
                     .Should()
                     .Throw<InvalidOperationException>();
    }

    [Fact]
    public void RemovePermission_GivenInput_Correctly()
    {
        var group = new PermissionGroup("name", "dispaly name");
        const string name = "Object1.Create";
        var permission = group.AddPermission(name, "create object1");
        group.RemovePermission(permission);

        group.Permissions.Should().BeEmpty();
    }

    [Fact]
    public void ToString_Should_ReturnCorrectly()
    {
        var group = new PermissionGroup("name", "dispaly name");
        var expected = $"PermissionGroup:{group.Name}";

        group.ToString().Should().Be(expected);
    }
}