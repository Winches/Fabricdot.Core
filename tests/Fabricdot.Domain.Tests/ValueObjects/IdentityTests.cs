using Fabricdot.Domain.ValueObjects;

namespace Fabricdot.Domain.Tests.ValueObjects;

public class IdentityTests : TestBase
{
    public class GuidIdentity : Identity<Guid>
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
        var sut = typeof(Identity<Guid>);

        Create<EqualityAssertion>().Verify(sut);
    }

    [Fact]
    public void Constructor_GivenInput_SetMember()
    {
        var sut = typeof(Identity<Guid>).GetConstructors();

        Create<ConstructorInitializedMemberAssertion>().Verify(sut);
    }
}