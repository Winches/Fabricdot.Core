using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.System.IO;

public class StreamExtensionsTests
{
    [Fact]
    public async Task GetBytesAsync_GivenNull_ThrowException()
    {
        await FluentActions.Awaiting(() => (null as Stream).GetBytesAsync())
                           .Should()
                           .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetBytesAsync_GivenStream_ReadAsBytes()
    {
        var expected = Encoding.UTF8.GetBytes("hello");
        using var sm = new MemoryStream(expected);
        var bytes = await sm.GetBytesAsync();

        bytes.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetStringAsync_GivenNull_ThrowException()
    {
        await FluentActions.Awaiting(() => (null as Stream).GetStringAsync())
               .Should()
               .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetStringAsync_GivenStream_ReadAsString()
    {
        const string expected = "hello";
        var encoding = Encoding.UTF8;
        using var sm = new MemoryStream(encoding.GetBytes(expected));
        var text = await sm.GetStringAsync(encoding);

        text.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetStringAsync_GivenNullEncoding_UseDefaultEncoding()
    {
        const string expected = "hello";
        var encoding = Encoding.UTF8;
        using var sm = new MemoryStream(encoding.GetBytes(expected));
        var text = await sm.GetStringAsync();

        text.Should().BeEquivalentTo(expected);
    }
}