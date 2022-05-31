using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Fabricdot.PermissionGranting.Tests;
using FluentAssertions;
using Xunit;

namespace Fabricdot.PermissionGranting.AspNetCore.Tests.Authoriation;

public class AuthorizationTests : IClassFixture<WebAppFactory<PermissionGrantingAspNetCoreTestModule>>
{
    private readonly WebAppFactory<PermissionGrantingAspNetCoreTestModule> _applicationFactory;
    private readonly HttpClient _httpClient;

    public AuthorizationTests(WebAppFactory<PermissionGrantingAspNetCoreTestModule> applicationFactory)
    {
        _applicationFactory = applicationFactory;
        _httpClient = _applicationFactory.CreateClient();
    }

    [Fact]
    public async Task RequestApi_WithoutPermission_Forbidden()
    {
        var httpResponseMessage = await _httpClient.GetAsync("/api/foo/shouldberejected");
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task RequestApi_WithPermission_Successful()
    {
        var httpResponseMessage = await _httpClient.GetAsync("/api/foo/shouldbeallowed");
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}