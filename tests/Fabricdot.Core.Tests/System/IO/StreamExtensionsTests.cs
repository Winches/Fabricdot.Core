using System.Text;

namespace Fabricdot.Core.Tests.System.IO;

public class StreamExtensionsTests : TestBase
{
    [Fact]
    public async Task GetBytesAsync_GivenNull_ThrowException()
    {
        await Awaiting(() => StreamExtensions.GetBytesAsync(null!))
                           .Should()
                           .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetBytesAsync_GivenStream_ReadAsBytes()
    {
        var expected = Create<byte[]>();
        using var sm = new MemoryStream(expected);
        var bytes = await sm.GetBytesAsync();

        bytes.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetStringAsync_GivenNull_ThrowException()
    {
        await Awaiting(() => StreamExtensions.GetStringAsync(null!))
               .Should()
               .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetStringAsync_GivenStream_ReadAsString()
    {
        var expected = Create<string>();
        var encoding = Create<Encoding>();
        using var sm = new MemoryStream(encoding.GetBytes(expected));
        var text = await sm.GetStringAsync(encoding);

        text.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetStringAsync_GivenNullEncoding_UseDefaultEncoding()
    {
        var expected = Create<string>();
        var encoding = Create<Encoding>();
        using var sm = new MemoryStream(encoding.GetBytes(expected));
        var text = await sm.GetStringAsync();

        text.Should().BeEquivalentTo(expected);
    }
}
