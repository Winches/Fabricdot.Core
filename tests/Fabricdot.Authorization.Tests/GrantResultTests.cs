namespace Fabricdot.Authorization.Tests;

public class GrantResultTests : TestFor<GrantResult>
{
    [Fact]
    public void Constructor_GivenInput_InitializeMember()
    {
        var sut = typeof(GrantResult).GetConstructors();

        Create<ConstructorInitializedMemberAssertion>().Verify(sut);
    }

    [Fact]
    public void EqualsOperator_GivenInput_ReturnCorrectly()
    {
        var grantResult = new GrantResult(Sut.Object, true);

        (Sut == null).Should().BeFalse();
        (Sut != null).Should().BeTrue();
        (Sut == grantResult).Should().BeTrue();
    }

    [Fact]
    public void Equality_GivenInput_ReturnCorrectly()
    {
        var sut = typeof(GrantResult);

        Create<EqualityAssertion>().Verify(sut);
    }
}
