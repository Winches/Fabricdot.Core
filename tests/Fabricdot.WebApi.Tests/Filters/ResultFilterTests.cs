using System.Net.Http.Json;
using Fabricdot.WebApi.Endpoint;
using Microsoft.AspNetCore.Http;

namespace Fabricdot.WebApi.Tests.Filters;

public class ResultFilterTests : WebApplicationTestBase<WebApiTestModule>
{
    protected const string BaseUrl = "api/fake/result-filter";

    public ResultFilterTests(TestWebApplicationFactory<WebApiTestModule> webAppFactory) : base(webAppFactory)
    {
    }

    [InlineData(StatusCodes.Status415UnsupportedMediaType)]
    [InlineData(StatusCodes.Status400BadRequest)]
    [Theory]
    public async Task FailedStatus_Should_Transformed(int statusCode)
    {
        var url = $"{BaseUrl}/failed-status?code={statusCode}";
        var response = await HttpClient.GetAsync(url);
        var res = await response.Content.ReadFromJsonAsync<Response<object>>();
        res.Should().BeEquivalentTo(new
        {
            Success = false,
            Code = statusCode
        });
    }

    [AutoData]
    [Theory]
    public async Task Response_Should_Ignored(int value, int code)
    {
        var url = $"{BaseUrl}/raw-type?value={value}&code={code}";
        var response = await HttpClient.GetAsync(url);
        var res = await response.Content.ReadFromJsonAsync<Response<object>>();
        res.Should().BeEquivalentTo(new
        {
            Success = true,
            Code = code
        });
    }
}
