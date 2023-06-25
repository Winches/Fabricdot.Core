namespace Fabricdot.Core.Tests.System.Reflection;

public class StringExtensionsTests : TestBase
{
    [InlineAutoData(null)]
    [InlineAutoData("")]
    [InlineAutoData]
    [Theory]
    public void IsNullOrEmpty_ShoulBeSameAsNativeMethod(string text)
    {
        var expected = string.IsNullOrEmpty(text);

        text.IsNullOrEmpty().Should().Be(expected);
    }

    [InlineAutoData(null)]
    [InlineAutoData("")]
    [InlineAutoData]
    [Theory]
    public void IsNullOrWhiteSpace_ShoulBeSameAsNativeMethod(string text)
    {
        var expected = string.IsNullOrEmpty(text);

        text.IsNullOrWhiteSpace().Should().Be(expected);
    }

    [InlineData(null, "-")]
    [InlineData("", "-")]
    [InlineData("test", null)]
    [Theory]
    public void RepeatString_GivenInput_RepeatString(string text, string separator)
    {
        var times = Create<int>();
        var repeatedText = text.Repeat(times, separator);
        var expected = string.Join(separator, Enumerable.Repeat(text, times));

        repeatedText.Should().Be(expected);
    }

    [Fact]
    public void RepeatString_GivenNegativeTimes_ThrowException()
    {
        Invoking(() => Create<string>().Repeat(-1))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [InlineData('t', null)]
    [InlineData('e', "-")]
    [Theory]
    public void RepeatChar_GivenInput_RepeatChar(char @char, string separator)
    {
        var times = Create<int>();
        var repeatedText = @char.Repeat(times, separator);
        var expected = string.Join(separator, Enumerable.Repeat(@char, times));

        repeatedText.Should().Be(expected);
    }

    [Fact]
    public void RepeatChar_GivenNegativeTimes_ThrowException()
    {
        Invoking(() => Create<char>().Repeat(-1))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [InlineData(null)]
    [InlineData("")]
    [Theory]
    public void Truncate_GivenNullOrEmpty_ReturnEmpty(string text)
    {
        text.Truncate(Create<int>()).Should().BeEmpty();
    }

    [AutoData]
    [Theory]
    public void Truncate_GivenLessLength_ReturnCorrectly(string text)
    {
        var expected = text.Length - 1;
        text.Truncate(expected).Should().HaveLength(expected);
    }

    [AutoData]
    [Theory]
    public void Truncate_GivenGreaterLength_ReturnCorrectly(string text)
    {
        var expected = text.Length;
        text.Truncate(text.Length + 1).Should().HaveLength(expected);
    }
}
