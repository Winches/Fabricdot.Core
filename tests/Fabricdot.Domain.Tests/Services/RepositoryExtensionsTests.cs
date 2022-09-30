using Ardalis.Specification;
using Fabricdot.Domain.Services;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.Domain.Tests.Services;

public class RepositoryExtensionsTests : TestFor<IReadOnlyRepository<Order>>
{
    public class OrderFilter : Specification<Order>
    {
    }

    [Fact]
    public async void AnyAsync_Should_Correctly()
    {
        var expected = await Sut.CountAsync() > 0;
        var actual = await Sut.AnyAsync();

        actual.Should().Be(expected);
    }

    [AutoMockData]
    [Theory]
    public async void AnyAsync_GivenSpecification_Correctly(OrderFilter specification)
    {
        var expected = await Sut.CountAsync(specification) > 0;
        var actual = await Sut.AnyAsync(specification);

        actual.Should().Be(expected);
    }
}