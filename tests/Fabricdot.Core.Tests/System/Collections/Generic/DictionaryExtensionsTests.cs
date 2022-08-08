namespace Fabricdot.Core.Tests.System.Collections.Generic;

public class DictionaryExtensionsTests : TestFor<Dictionary<string, int>>
{
    [Fact]
    public void GetOrDefault_WhenValueIsNotNull_ReturnValue()
    {
        var key = Sut.Keys.First();
        var value = Sut[key];

        Sut.GetOrDefault(key).Should().Be(value);
    }

    [Fact]
    public void GetOrDefault_WhenValueIsNull_ReturnDefaultValue()
    {
        var key = Create<string>();

        Sut.GetOrDefault(key).Should().Be(default);
    }

    [Fact]
    public void GetOrAdd_WhenKeyExists_ReturnValue()
    {
        var key = Sut.Keys.First();
        var value = Sut[key];

        Sut.GetOrAdd(key, _ => value).Should().Be(value);
    }

    [AutoData]
    [Theory]
    public void GetOrAdd_WhenKeyNotExists_AddValue(
        string key,
        int value)
    {
        Sut.GetOrAdd(key, _ => value).Should().Be(value);
    }
}