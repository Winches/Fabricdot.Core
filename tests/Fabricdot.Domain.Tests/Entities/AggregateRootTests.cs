using Fabricdot.Domain.Events;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
namespace Fabricdot.Domain.Tests.Entities;

public class AggregateRootTests : TestFor<Order>
{
    [Fact]
    public void New_GenerateConcurrencyStamp()
    {
        Sut.ConcurrencyStamp.Should().NotBeEmpty();
    }

    [Fact]
    public void AddDomainEvent_GivenEvent_Correctly()
    {
        var expected = Create<IDomainEvent>();
        Sut.AddDomainEvent(expected);

        Sut.DomainEvents.Should().Contain(expected);
    }

    [Fact]
    public void AddDomainEvent_GivenNull_ThrownException()
    {
        var sut = typeof(Order).GetMethod(nameof(Order.AddDomainEvent));

        Create<GuardClauseAssertion>().Verify(sut);
    }

    [Fact]
    public void RemoveDomainEvent_GivenEvent_Correctly()
    {
        var expected = Create<IDomainEvent>();
        Sut.AddDomainEvent(expected);
        Sut.RemoveDomainEvent(expected);

        Sut.DomainEvents.Should().NotContain(expected);
    }

    [Fact]
    public void ClearDomainEvents_EmptyEvents()
    {
        Sut.AddDomainEvent(Create<IDomainEvent>());
        Sut.ClearDomainEvents();

        Sut.DomainEvents.Should().BeEmpty();
    }
}
