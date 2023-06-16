using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.Domain.Tests.ValueObjects;

public class ValueObjectTests : TestFor<Address>
{
    [Fact]
    public void Equality_Should_Correctly()
    {
        var sut = typeof(Address);

        Create<EqualityAssertion>().Verify(sut);
    }

    [Fact]
    public void EqualsOperator_GivenSameValueInstance_ReturnTrue()
    {
        var address = new Address(Sut.State, Sut.City, Sut.Street);

        (Sut == address).Should().BeTrue();
        (Sut != address).Should().BeFalse();
    }

    [Fact]
    public void EqualsOperator_GivenDifferentValueInstance_ReturnFalse()
    {
        var address = Create<Address>();

        (Sut == address).Should().BeFalse();
        (Sut != address).Should().BeTrue();
    }

    [Fact]
    public void EqualsOperator_GivenNull_ReturnFalse()
    {
        (Sut == null).Should().BeFalse();
        (Sut != null).Should().BeTrue();
    }

    [Fact]
    public void ToString_ReturnCorrectly()
    {
        var expected = $"{{{nameof(Address.City)}: {Sut.City},{nameof(Address.State)}: {Sut.State},{nameof(Address.Street)}: {Sut.Street}}}";

        Sut.ToString().Should().Be(expected);
    }
}
