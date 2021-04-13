using System;
using Fabricdot.Infrastructure.Core.Domain.Events;
using Xunit;

namespace Fabricdot.Infrastructure.Core.Tests.Domain.Events
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