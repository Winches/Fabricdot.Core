using Fabricdot.Core.Validation;

namespace Fabricdot.Core.Tests.Validation;

public class NotificationErrorTests : TestBase
{
    [Fact]
    public void Format_GivenInput_ReturnCorrectly()
    {
        var message = $"{Create<string>()} {0}";
        var arg = Create<string>();
        var expected = string.Format(message, arg);
        var error = new Notification.Error(message);

        error.Format(arg).ToString().Should().Be(expected);
    }
}