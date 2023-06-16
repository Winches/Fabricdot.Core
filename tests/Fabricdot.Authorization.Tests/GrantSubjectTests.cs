using System.Security.Claims;

namespace Fabricdot.Authorization.Tests;

public class GrantSubjectTests : TestBase
{
    [Fact]
    public void Constructor_GivenInvalidInput_Throw()
    {
        var sut = typeof(GrantSubject).GetConstructors();

        Create<GuardClauseAssertion>().Verify(sut);
    }

    [AutoMockData]
    [Theory]
    public void ConversionOperator_GivenClaim_Correctly(Claim claim)
    {
        GrantSubject subject = claim;

        subject.Should().BeEquivalentTo(claim, opts => opts.ExcludingMissingMembers());
    }

    [AutoData]
    [Theory]
    public void User_Should_ReturnCorrectly(string userId)
    {
        var subject = GrantSubject.User(userId);
        var expected = new
        {
            Type = GrantTypes.User,
            Value = userId
        };

        subject.Should().BeEquivalentTo(expected);
    }

    [AutoData]
    [Theory]
    public void Role_Should_ReturnCorrectly(string role)
    {
        var subject = GrantSubject.Role(role);
        var expected = new
        {
            Type = GrantTypes.Role,
            Value = role
        };

        subject.Should().BeEquivalentTo(expected);
    }
}
