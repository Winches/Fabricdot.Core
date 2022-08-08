namespace Fabricdot.Core.Tests.System.Collections.Generic;

public class EnumerableExtensionsTests : TestFor<List<string>>
{
    public static IEnumerable<object[]> GetEnumrables()
    {
        yield return new object[] { null };
        yield return new object[] { Array.Empty<object>() };
        yield return new object[] { new List<object>() };
        yield return new object[] { new object[] { 1 } };
    }

    [MemberData(nameof(GetEnumrables))]
    [Theory]
    public void IsNullOrEmpty_GivenInput_ReturnCorrectly(IEnumerable<object> enumerable)
    {
        var isNullOrEmpty = enumerable?.Any() != true;

        enumerable.IsNullOrEmpty().Should().Be(isNullOrEmpty);
    }

    [Fact]
    public void ForEach_GivenAction_IteratingElements()
    {
        var count = 0;
        Sut.ForEach((v, i) =>
        {
            count++;
            v.Should().Be(Sut[i]);
        });

        count.Should().Be(Sut.Count);
    }

    [Fact]
    public async Task ForEachAsync_GivenAction_IteratingElementsAsync()
    {
        var count = 0;
        await Sut.ForEachAsync((v, i) =>
        {
            count++;
            v.Should().Be(Sut[i]);
            return Task.Delay(100);
        });

        count.Should().Be(Sut.Count);
    }

    [AutoData]
    [Theory]
    public void JoinAsString_GivenInput_JoinString(
        string[] source,
        string separator1,
        char separator2)
    {
        var expected1 = string.Join(separator1, source);
        var expected2 = string.Join(separator2, source);

        source.JoinAsString(separator1)
              .Should()
              .Be(expected1);
        source.JoinAsString(separator2)
              .Should()
              .Be(expected2);
    }
}