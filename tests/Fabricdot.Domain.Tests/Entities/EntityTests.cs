using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.Domain.Tests.Entities;

public class EntityTests : TestFor<Order>
{
    [Fact]
    public void Equals_GivenInstanceWithSameId_ReturnTrue()
    {
        var order = new Order(Sut.Id, Create<Address>(), Create<string>(), null);

        Sut.Should().Be(order);
    }

    [Fact]
    public void Equals_GivenInstanceWithDifferentId_ReturnFalse()
    {
        var order = Create<Order>();

        Sut.Should().NotBe(order);
    }

    [Fact]
    public void Equality_Should_Correctly()
    {
        var sut = typeof(Order);

        Create<EqualityAssertion>().Verify(sut);
    }

    [Fact]
    public void ToString_Should_ReturnCorrectly()
    {
        var expected = $"Entity:{Sut.GetType().Name}, Id:{Sut.Id}";

        Sut.ToString().Should().Be(expected);
    }
}
