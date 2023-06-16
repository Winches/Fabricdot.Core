using Fabricdot.Core.Boot;

namespace Fabricdot.Core.Tests.Boot;

public class ApplicationTests : TestFor<Application>
{
    [Fact]
    public async Task StartAsync_Should_Correctly()
    {
        await Sut.StartAsync();
    }

    [Fact]
    public async Task StopAsync_Should_Correctly()
    {
        await Sut.StopAsync();
    }
}
