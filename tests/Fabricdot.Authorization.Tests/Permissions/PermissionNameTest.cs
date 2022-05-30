using System;
using Fabricdot.Authorization.Permissions;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Authorization.Tests.Permissions
{
    public class PermissionNameTest
    {
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [Theory]
        public void Constructor_GivenInvalidInput_Throw(string value)
        {
            FluentActions.Invoking(() => new PermissionName(value))
                         .Should()
                         .Throw<ArgumentException>();
        }

        [Fact]
        public void Equal_Should_ReturnCorrectly()
        {
            var name1 = new PermissionName("name1");
            var name2 = new PermissionName(name1.Value);

            name1.Equals(null).Should().BeFalse();
            (name1 != name2).Should().BeFalse();
            (name1 == name2).Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_Should_ReturnCorrectly()
        {
            var name1 = new PermissionName("name1");
            var expected = name1.Value.GetHashCode();

            name1.GetHashCode().Should().Be(expected);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectly()
        {
            var name = new PermissionName("name1");

            name.ToString().Should().Be(name.Value);
        }

        [Fact]
        public void ConversionOperator_Should_ReturnCorrectly()
        {
            PermissionName name = "name1";
            string nameString = name;

            name.Should().BeOfType<PermissionName>();
            nameString.Should().BeOfType<string>();
        }
    }
}