using Fabricdot.Domain.ValueObjects;

namespace Fabricdot.Domain.Tests.ValueObjects;

public class IdentityTests : TestBase
{
    public class GuidIdentity : Identity
    {
        public GuidIdentity(Guid value) : base(value)
        {
        }
    }

    public class StringIdenttiy : Identity<string>
    {
        public StringIdenttiy(string value) : base(value)
        {
        }
    }

    public class LongIdenttiy : Identity<long>
    {
        public LongIdenttiy(long value) : base(value)
        {
        }
    }

    [Fact]
    public void Equality_Should_Correctly()
    {
        var sut = typeof(Identity);

        Create<EqualityAssertion>().Verify(sut);
    }

    [InlineData(typeof(GuidIdentity))]
    [InlineData(typeof(StringIdenttiy))]
    [InlineData(typeof(LongIdenttiy))]
    [Theory]
    public void Constructor_Should_Correctly(Type type)
    {
        var sut = type.GetConstructors();

        Create<GuardClauseAssertion>().Verify(sut);
    }

    [Fact]
    public void Constructor_GivenInput_SetMember()
    {
        var sut = typeof(Identity).GetConstructors();

        Create<ConstructorInitializedMemberAssertion>().Verify(sut);
    }

    [AutoMockData]
    [Theory]
    public void ToString_Should_Correctly(Identity sut)
    {
        var expected = sut.Value.ToString();

        sut.ToString().Should().Be(expected);
    }
}
