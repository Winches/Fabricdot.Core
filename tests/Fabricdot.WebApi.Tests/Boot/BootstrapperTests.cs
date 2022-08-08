namespace Fabricdot.WebApi.Tests.Boot;

public class BootstrapperTests : WebApplicationTestBase<WebApiTestModule>
{
    public BootstrapperTests(TestWebApplicationFactory<WebApiTestModule> webAppFactory) : base(webAppFactory)
    {
    }

    [Fact]
    public async Task Bootstrap_CorrectlyAsync()
    {
        _ = await HttpClient.GetAsync("/");
    }
}