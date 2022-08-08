namespace Fabricdot.Core.Tests.System.Reflection;

public class DateTimeExtensionsTests : TestBase
{
    [Fact]
    public void ToDateTime_GivenInvalidTimestamp_ThrowException()
    {
        Invoking(() => (-1d).ToDateTime())
                     .Should()
                     .Throw<ArgumentException>();
    }

    [InlineData(0)]
    [InlineData(10000)]
    [Theory]
    public void ToDateTime_GivenTimestamp_CreateDatetime(double timestamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var expected = dateTime.AddMilliseconds(timestamp).ToLocalTime();

        timestamp.ToDateTime().Should().Be(expected);
    }

    [Fact]
    public void ToTimestamp_GivenDateTime_CreateTimestamp()
    {
        var time = DateTime.UtcNow;
        var expected = (long)(time - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        time.ToTimestamp().Should().Be(expected);
    }
}