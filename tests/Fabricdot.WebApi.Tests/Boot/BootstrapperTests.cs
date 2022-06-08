using System.Threading.Tasks;
using Xunit;

namespace Fabricdot.WebApi.Tests.Boot;

public class BootstrapperTests : AspNetCoreTestsBase<ModularityStartup<StartupModule>>
{
    [Fact]
    public async Task Bootstrap_CorrectlyAsync()
    {
        var response = await HttpClient.GetAsync("/");
    }
}