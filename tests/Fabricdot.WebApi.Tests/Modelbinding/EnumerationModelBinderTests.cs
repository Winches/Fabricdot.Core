using System.Net;
using System.Net.Http.Json;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Fabricdot.WebApi.Endpoint;

namespace Fabricdot.WebApi.Tests.Modelbinding;

public class EnumerationModelBinderTests : ModelBinderTestBase
{
    protected const string BaseUrl = "api/fake/model-binding/enumeration";

    public EnumerationModelBinderTests(TestWebApplicationFactory<WebApiTestModule> webAppFactory) : base(webAppFactory)
    {
    }

    [Fact]
    public async Task FromQuery_GivenNull_DoNotThrow()
    {
        const string url = BaseUrl + "/query?orderstatus=";
        var response = await HttpClient.GetAsync(url);

        response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task FromQuery_GivenValue_Correctly()
    {
        var url = $"{BaseUrl}/query?orderstatus={OrderStatus.Placed.Value}";
        var response = await HttpClient.GetAsync(url);

        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task FromRoute_GivenValue_Correctly()
    {
        var url = $"{BaseUrl}/route/{OrderStatus.Placed.Value}";
        var response = await HttpClient.GetAsync(url);

        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task FromForm_GivenValue_Correctly()
    {
        const string url = $"{BaseUrl}/form";
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Status"] = OrderStatus.Placed.Value.ToString()
        });
        var response = await HttpClient.PostAsync(url, content);
        var res = await response.Content.ReadFromJsonAsync<Response<object>>();

        response.Should().BeSuccessful();
        res.Success.Should().BeTrue();
    }

    [Fact]
    public async Task FromJson_GivenValue_Correctly()
    {
        const string url = $"{BaseUrl}/json";
        var content = JsonContent.Create(new { Status = OrderStatus.Completed.Value });
        var response = await HttpClient.PostAsync(url, content);
        var res = await response.Content.ReadFromJsonAsync<Response<object>>();

        response.Should().BeSuccessful();
        res.Success.Should().BeTrue();
    }
}
