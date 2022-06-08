using System;
using Fabricdot.Core.UniqueIdentifier;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.UniqueIdentifier;

public class DefaultCombGuidTimestampProviderTests
{
    [Fact]
    public void GetTimestamp_ShouldReturnUtcTimestamp()
    {
        var before = DateTime.UtcNow.Ticks / 10000L;
        var timestamp = DefaultCombGuidTimestampProvider.Instance.GetTimestamp();
        var after = DateTime.UtcNow.Ticks / 10000L;

        timestamp.Should()
                 .BeGreaterThanOrEqualTo(before).And
                 .BeLessThanOrEqualTo(after);
    }
}