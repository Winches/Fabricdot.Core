using System.Net;
using Fabricdot.PermissionGranting.Tests;

namespace Fabricdot.PermissionGranting.AspNetCore.Tests.Authoriation;

public class AuthorizationTests : WebApplicationTestBase<PermissionGrantingAspNetCoreTestModule>
{
    public AuthorizationTests(TestWebApplicationFactory<PermissionGrantingAspNetCoreTestModule> webAppFactory) : base(webAppFactory)
    {
    }

    [Fact]
    public async Task RequestApi_WithoutPermission_Forbidden()
    {
        var httpResponseMessage = await HttpClient.GetAsync("/api/foo/shouldberejected");

        httpResponseMessage.Should().HaveStatusCode(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task RequestApi_WithPermission_Successful()
    {
        var httpResponseMessage = await HttpClient.GetAsync("/api/foo/shouldbeallowed");

        httpResponseMessage.Should().HaveStatusCode(HttpStatusCode.OK);
    }
}