using Fabricdot.Core.Modularity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace Fabricdot.AspNetCore.Testing;

public abstract class WebApplicationTestBase<TModule> : TestBase, IClassFixture<TestWebApplicationFactory<TModule>> where TModule : class, IModule
{
    private HttpClient _httpClient;

    protected WebApplicationFactory<TestStartup<TModule>> WebAppFactory { get; set; }

    protected TestServer Server => WebAppFactory.Server;

    protected IServiceProvider ServiceProvider => WebAppFactory.Services;

    protected HttpClient HttpClient => _httpClient ??= CreateClient();

    protected WebApplicationTestBase(TestWebApplicationFactory<TModule> webAppFactory)
    {
        WebAppFactory = webAppFactory;
    }

    protected virtual void WithWebHostBuilder()
    {
    }

    protected virtual HttpClient CreateClient(WebApplicationFactoryClientOptions options = null)
    {
        return options is null ? WebAppFactory.CreateClient() : WebAppFactory.CreateClient(options);
    }
}