using Fabricdot.Core.Validation;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.Validation
{
    public class NotificationErrorTests
    {
        [Fact]
        public void Format_GivenInput_ReturnCorrectly()
        {
            const string message = "Hello {0}!";
            const string arg = "World";
            var expected = string.Format(message, arg);
            var error = new Notification.Error(message);

            error.Format(arg).ToString().Should().Be(expected);
        }
    }
}