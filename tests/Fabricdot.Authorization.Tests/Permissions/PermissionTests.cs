using System;
using System.Linq;
using Fabricdot.Authorization.Permissions;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionTests
{
    [Fact]
    public void Constructor_GivenInput_Correctly()
    {
        var permission1 = new Permission("name1", "name 1", null, null);
        var permission2 = new Permission("name2", "name 2", null, new[]
        {
            permission1
        });

        permission1.Children.Should().BeEmpty();
        permission2.Children.Single().Should().Be(permission1);
    }

    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [Theory]
    public void DisplyName_GivenInvalidInput_Throw(string dispalyName)
    {
        FluentActions.Invoking(() => new Permission("name", dispalyName))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [Fact]
    public void Add_GivenInput_Correctly()
    {
        var expected = new
        {
            Name = new PermissionName("name1.name2"),
            DisplayName = "name 2",
            Description = "description"
        };
        var permission = new Permission("name1", "name 1");

        var self = permission.Add(
            expected.Name,
            expected.DisplayName,
            expected.Description);

        self.Should().BeSameAs(permission);
        self.Children.Single().Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Remove_GivenInput_Correctly()
    {
        var permission = new Permission("name1", "name 1");
        var name = new PermissionName("name1.name2");
        permission.Add(name, "name2");
        permission.Remove(name);

        permission.Children.Should().BeEmpty();
    }

    [Fact]
    public void Equal_Should_ReturnCorrectly()
    {
        const string name = "name1";
        var permission1 = new Permission(name, "name 1");
        var permission2 = new Permission(name, "name 2");

        permission1.Equals(null).Should().BeFalse();
        permission1.Equals(permission2).Should().BeTrue();
    }

    [Fact]
    public void ToString_Should_ReturnCorrectly()
    {
        var permission = new Permission("name1", "name 1");
        var expected = $"Permission:{permission.Name}";

        permission.ToString().Should().Be(expected);
    }
}