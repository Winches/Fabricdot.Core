using Fabricdot.Core.UniqueIdentifier.CombGuid;

namespace Fabricdot.Core.Tests.UniqueIdentifier;

public class SafetyCombGuidTimestampProviderTests : TestFor<SafetyCombGuidTimestampProvider>
{
    [Fact]
    public void GetTimestamp_ReturnUniqueTimestamp()
    {
        var timestamps = Enumerable.Range(1, 1000)
                      .Select(_ => Sut.GetTimestamp())
                      .ToArray();

        timestamps.Should().OnlyHaveUniqueItems();
    }

    protected override SafetyCombGuidTimestampProvider CreateSut() => SafetyCombGuidTimestampProvider.Instance;
}