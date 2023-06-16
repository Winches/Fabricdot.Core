using Fabricdot.Domain.ValueObjects;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.Domain.Tests.ValueObjects;

public class SingleValueObjectTests : TestFor<Money>
{
    internal class AttributeTargetsValue : SingleValueObject<AttributeTargets>
    {
        public AttributeTargetsValue(AttributeTargets value) : base(value)
        {
        }
    }

    [Fact]
    public void GetValue_Should_Correctly()
    {
        var expected = Sut.Value;

        Sut.GetValue().Should().Be(expected);
    }

    [Fact]
    public void Constructor_GivenInvalidEnum_Throw()
    {
        Invoking(() => new AttributeTargetsValue(99.Cast<AttributeTargets>())).Should()
                                                                              .Throw<ArgumentException>();
    }

    [Fact]
    public void Equality_Should_Correctly()
    {
        var sut = typeof(Money);

        Create<EqualityAssertion>().Verify(sut);
    }

    [InlineAutoData]
    [InlineAutoData(null)]
    [Theory]
    public void CompareTo_Should_Correctly(Money money)
    {
        var expected = Sut.Value.CompareTo(money?.Value);

        Sut.CompareTo(money).Should().Be(expected);
    }

    [Fact]
    public void CompareTo_GivenDifferentType_Throw()
    {
        Invoking(() => Sut.CompareTo(Create<object>())).Should()
                                                       .Throw<ArgumentException>();
    }
}
