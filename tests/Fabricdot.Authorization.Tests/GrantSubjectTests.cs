using System;
using System.Security.Claims;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Authorization.Tests
{
    public class GrantSubjectTests
    {
        [InlineData(null, "value")]
        [InlineData("", "value")]
        [InlineData(" ", "value")]
        [InlineData("type", null)]
        [InlineData("type", "")]
        [InlineData("type", " ")]
        [Theory]
        public void Constructor_GivenInvalidInput_Throw(
            string type,
            string value)
        {
            FluentActions.Invoking(() => new GrantSubject(type, value))
                         .Should()
                         .Throw<ArgumentException>();
        }

        [Fact]
        public void ConversionOperator_GivenClaim_Correctly()
        {
            var claim = new Claim("type", "value");
            GrantSubject subject = claim;

            subject.Should().BeEquivalentTo(claim, opts => opts.ExcludingMissingMembers());
        }

        [Fact]
        public void User_Should_ReturnCorrectly()
        {
            const string value = "1";
            var subject = GrantSubject.User(value);
            var expected = new
            {
                Type = GrantTypes.User,
                Value = value
            };

            subject.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Role_Should_ReturnCorrectly()
        {
            const string value = "role1";
            var subject = GrantSubject.Role(value);
            var expected = new
            {
                Type = GrantTypes.Role,
                Value = value
            };

            subject.Should().BeEquivalentTo(expected);
        }
    }

    public class GrantResultTests
    {
        [Fact]
        public void GetHashCode_Should_ReturnCorrectly()
        {
            var grantResult = new GrantResult("object");
            var expected = grantResult.Object.GetHashCode();

            grantResult.GetHashCode().Should().Be(expected);
        }

        [Fact]
        public void Equal_GivenInput_ReturnCorrectly()
        {
            var grantResult1 = new GrantResult("object");
            var grantResult2 = new GrantResult(grantResult1.Object, true);

            (grantResult1 == null).Should().BeFalse();
            (grantResult1 != null).Should().BeTrue();
            (grantResult1 == grantResult2).Should().BeTrue();
        }
    }
}