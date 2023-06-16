using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.Domain.Tests.ValueObjects;

public class ValueObjectTest_IgnoreMember_Tests : TestFor<Address>
{
    [Fact]
    public void Equal_GivenDifferentValue_IgnoreMember()
    {
        var value2 = new Address(Sut.State, Sut.City, Sut.Street, Create<string>());

        Sut.Should().Be(value2);
    }
}
