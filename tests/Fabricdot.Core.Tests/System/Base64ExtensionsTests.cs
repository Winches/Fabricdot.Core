using System.Text;

namespace Fabricdot.Core.Tests.System.Reflection;

public class Base64ExtensionsTests : TestBase
{
    public static IEnumerable<object[]> GetData()
    {
        // http://tools.ietf.org/html/rfc4648
        yield return new object[] { "", "" };
        yield return new object[] { "f", "Zg==" };
        yield return new object[] { "fo", "Zm8=" };
        yield return new object[] { "foo", "Zm9v" };
        yield return new object[] { "foob", "Zm9vYg==" };
        yield return new object[] { "fooba", "Zm9vYmE=" };
        yield return new object[] { "foobar", "Zm9vYmFy" };
    }

    [InlineData(null)]
    [InlineData("")]
    [Theory]
    public void ToBase64_GivenNullOrEmpty_ReturnEmpty(string text)
    {
        text.ToBase64().Should().BeEmpty();
    }

    [MemberData(nameof(GetData))]
    [Theory]
    public void ToBase64_GivenInput_ReturnCorrectly(string text, string expected)
    {
        var encoding = Encoding.UTF8;

        text.ToBase64(encoding).Should().Be(expected);
    }

    [InlineData(null)]
    [InlineData("")]
    [Theory]
    public void FromBase64_GivenNullOrEmpty_ReturnEmpty(string text)
    {
        text.FromBase64().Should().BeEmpty();
    }

    [InlineData("aGVsbG8sIHdvcmxk=")]
    [InlineData("aGVsbG8sIHdvcmxk==")]
    [InlineData("aGVsbG8sIHdvcmxk =")]
    [InlineData("aGVsbG8sIHdvcmxk = = ")]
    [Theory]
    public void FromBase64_GivenInvalidInput_Throw(string text)
    {
        Invoking(() => text.FromBase64()).Should().Throw<ArgumentException>();
    }

    [MemberData(nameof(GetData))]
    [Theory]
    public void FromBase64_GivenInput_ReturnCorrectly1(
        string expected,
        string encoded)
    {
        var encoding = Encoding.UTF8;
        encoded.FromBase64(encoding).Should().Be(expected);
    }
}
