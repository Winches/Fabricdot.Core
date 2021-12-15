using System;
using Fabricdot.Infrastructure.Domain.Events;
using Xunit;

namespace Fabricdot.Infrastructure.Tests.Domain.Events
{
    public class DomainEventNotificationTests
    {
        [Fact]
        public void New_GivenNull_ThrowException()
        {
            static void Action()
            {
                var _ = new DomainEventNotification(null);
            }
            Assert.Throws<ArgumentNullException>(Action);
        }
    }
}