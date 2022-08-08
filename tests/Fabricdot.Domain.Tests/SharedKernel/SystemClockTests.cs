using Fabricdot.Domain.SharedKernel;

namespace Fabricdot.Domain.Tests.SharedKernel;

public class SystemClockTests
{
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Local)]
    [Theory]
    public void Now_ConfigureDatetimeKind_ReturnCorrectly(DateTimeKind dateTimeKind)
    {
        SystemClock.Configure(dateTimeKind);

        SystemClock.Kind.Should().Be(dateTimeKind);
        SystemClock.Now.Kind.Should().Be(dateTimeKind);
    }

    [Fact]
    public void Normalize_ConfigureUtcGivenLocalTime_ReturnUtcTime()
    {
        const DateTimeKind expected = DateTimeKind.Utc;
        SystemClock.Configure(expected);

        SystemClock.Normalize(DateTime.Now).Kind.Should().Be(expected);
    }

    [Fact]
    public void Normalize_ConfigureUtcGivenUnspecifiedTime_ReturnUtcTime()
    {
        const DateTimeKind expected = DateTimeKind.Utc;
        SystemClock.Configure(expected);

        SystemClock.Normalize(new DateTime()).Kind.Should().Be(expected);
    }

    [Fact]
    public void Normalize_ConfigureLocalGivenUtcTime_ReturnLocalTime()
    {
        const DateTimeKind expected = DateTimeKind.Local;
        SystemClock.Configure(expected);

        SystemClock.Normalize(DateTime.UtcNow).Kind.Should().Be(expected);
    }

    [Fact]
    public void Normalize_ConfigureLocalGivenUnspecifiedTime_ReturnLocalTime()
    {
        const DateTimeKind expected = DateTimeKind.Local;
        SystemClock.Configure(expected);

        SystemClock.Normalize(new DateTime()).Kind.Should().Be(expected);
    }

    [Fact]
    public void Normalize_ConfigureUnspecifiedGivenDateTime_ReturnGivenTime()
    {
        SystemClock.Configure(DateTimeKind.Unspecified);
        var utcTime = SystemClock.Normalize(DateTime.UtcNow);
        var localTime = SystemClock.Normalize(DateTime.Now);

        utcTime.Kind.Should().Be(DateTimeKind.Utc);
        localTime.Kind.Should().Be(DateTimeKind.Local);
    }
}