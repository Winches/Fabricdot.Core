using Fabricdot.Domain.ValueObjects;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.Domain.Tests.ValueObjects;

public class EnumerationTests : TestFor<OrderStatus>
{
    [Fact]
    public void Equality_Should_Correctly()
    {
        var sut = typeof(OrderStatus);

        Create<EqualityAssertion>().Verify(sut);
    }

    [Fact]
    public void Equals_GivenSameValueDifferentNameInstance_ReturnTrue()
    {
        var status = new OrderStatus(Sut.Value, Create<string>());

        Sut.Should().Be(status);
    }

    [Fact]
    public void EqualsOperator_GivenSameValue_Correctly()
    {
        var status = new OrderStatus(Sut.Value, Sut.Name);

        (status == Sut).Should().BeTrue();
        (status != Sut).Should().BeFalse();
    }

    [Fact]
    public void EqualsOperator_GivenDifferentValue_Correctly()
    {
        var status = Create<OrderStatus>();

        (status == Sut).Should().BeFalse();
        (status != Sut).Should().BeTrue();
    }

    [Fact]
    public void EqualsOperator_GivenNull_Correctly()
    {
        (Sut == null).Should().BeFalse();
        (Sut != null).Should().BeTrue();
    }

    [InlineAutoData]
    [InlineAutoData(null)]
    [Theory]
    public void Compare_Should_Correctly(OrderStatus status)
    {
        var expected = Sut.Value.CompareTo(status?.Value);

        Sut.CompareTo(status).Should().Be(expected);
    }

    [Fact]
    public void CompareTo_GivenDifferentType_Throw()
    {
        Invoking(() => Sut.CompareTo(Create<object>())).Should()
                                                       .Throw<ArgumentException>();
    }

    [AutoData]
    [Theory]
    public void AbsoluteDifference_GivenInstance_ReturnAbsoluteValue(OrderStatus status)
    {
        var expected = Math.Abs(Sut.Value - status.Value);

        Enumeration.AbsoluteDifference(Sut, status).Should().Be(expected);
    }

    [Fact]
    public void GetAll_ReturnPublicStaticFields()
    {
        var values = Enumeration.GetAll<OrderStatus>().ToList();

        values.Should().Contain(OrderStatus.Placed);
        values.Should().Contain(OrderStatus.Shipped);
        values.Should().Contain(OrderStatus.Completed);
    }

    [Fact]
    public void FromValue_GivenExistedValue_ReturnInstance()
    {
        var expected = OrderStatus.Placed;

        Enumeration.FromValue<OrderStatus>(expected.Value).Should().Be(expected);
    }

    [Fact]
    public void FromValue_GivenNotExistedValue_ThrowException()
    {
        Invoking(() => Enumeration.FromValue<OrderStatus>(0))
                     .Should()
                     .Throw<InvalidOperationException>();
    }

    [Fact]
    public void FromName_GivenExistedName_ReturnInstance()
    {
        var expected = OrderStatus.Placed;

        Enumeration.FromName<OrderStatus>(expected.Name).Should().Be(expected);
    }

    [Fact]
    public void FromName_GivenNotExistedName_ThrowException()
    {
        Invoking(() => Enumeration.FromName<OrderStatus>(Create<string>()))
                     .Should()
                     .Throw<InvalidOperationException>();
    }
}
