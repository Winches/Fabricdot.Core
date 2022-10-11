using Fabricdot.Infrastructure.Data;
using Microsoft.Extensions.Options;

namespace Fabricdot.Infrastructure.Tests.Data;

public class DefaultConnectionStringResolverTests : TestFor<DefaultConnectionStringResolver>
{
    private readonly IOptionsSnapshot<ConnectionOptions> _options;

    public DefaultConnectionStringResolverTests()
    {
        _options = Create<IOptionsSnapshot<ConnectionOptions>>();
        Inject(_options);
    }

    [AutoData]
    [InlineData("")]
    [Theory]
    public async Task ResolveAsync_GivenInvalidName_ReturnNull(string name)
    {
        var actual = await Sut.ResolveAsync(name);

        actual.Should().BeNull();
    }

    [AutoData]
    [Theory]
    public async Task ResolveAsync_GivenNull_ReturnDefault(string expected)
    {
        _options.Value.ConnectionStrings.Default = expected;
        var actual = await Sut.ResolveAsync();

        actual.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task ResolveAsync_GivenName_ReturnCorrectly(
        string name,
        string expected)
    {
        _options.Value.ConnectionStrings[name] = expected;
        var actual = await Sut.ResolveAsync(name);

        actual.Should().Be(expected);
    }
}