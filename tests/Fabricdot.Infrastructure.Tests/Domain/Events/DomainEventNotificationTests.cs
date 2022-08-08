using Fabricdot.Infrastructure.Domain.Events;

namespace Fabricdot.Infrastructure.Tests.Domain.Events;

public class DomainEventNotificationTests : TestFor<DomainEventNotification>
{
    [Fact]
    public void New_GivenNull_ThrowException()
    {
        var sut = typeof(DomainEventNotification).GetConstructors();

        Create<GuardClauseAssertion>().Verify(sut);
    }

    [Fact]
    public void Constructor_Should_Correctly()
    {
        Sut.Event.Should().NotBeNull();
    }
}