using Fabricdot.Core.UniqueIdentifier;

namespace Fabricdot.Core.Tests.UniqueIdentifier;

public class DefaultCombGuidTimestampProviderTests : TestFor<DefaultCombGuidTimestampProvider>
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

    protected override DefaultCombGuidTimestampProvider CreateSut() => DefaultCombGuidTimestampProvider.Instance;
}