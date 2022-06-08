using System.Threading.Tasks;
using Fabricdot.Core.Boot;
using Xunit;

namespace Fabricdot.Core.Tests.Boot;

public class ApplicationTests
{
    private class FakeApplication : Application
    {
    }

    [Fact]
    public async Task StartAsync_DoNothing()
    {
        await new FakeApplication().StartAsync();
    }

    [Fact]
    public async Task StopAsync_DoNothing()
    {
        await new FakeApplication().StopAsync();
    }
}