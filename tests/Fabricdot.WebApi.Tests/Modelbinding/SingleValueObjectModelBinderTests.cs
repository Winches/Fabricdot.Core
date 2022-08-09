using System.Net;
using System.Net.Http.Json;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Fabricdot.WebApi.Endpoint;

namespace Fabricdot.WebApi.Tests.Modelbinding;

public class SingleValueObjectModelBinderTests : ModelBinderTestBase
{
    protected const string BaseUrl = "api/fake/model-binding/singlevaleobject";

    public SingleValueObjectModelBinderTests(TestWebApplicationFactory<WebApiTestModule> webAppFactory) : base(webAppFactory)
    {
    }

    [Fact]
    public async Task FromQuery_GivenNull_DoNotThrow()
    {
        const string url = $"{BaseUrl}/query?money=";
        var response = await HttpClient.GetAsync(url);

        response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
    }

    [AutoData]
    [Theory]
    public async Task FromQuery_GivenValue_Correctly(Money money)
    {
        var url = $"{BaseUrl}/query?money={money}";
        var response = await HttpClient.GetAsync(url);

        response.Should().BeSuccessful();
    }

    [AutoData]
    [Theory]
    public async Task FromRoute_GivenValue_Correctly(Money money)
    {
        var url = $"{BaseUrl}/route/{money}";
        var response = await HttpClient.GetAsync(url);

        response.Should().BeSuccessful();
    }


    [AutoData]
    [Theory]
    public async Task FromForm_GivenValue_Correctly(Money money)
    {
        const string url = $"{BaseUrl}/form";
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Amount"] = money.ToString()
        });
        var response = await HttpClient.PostAsync(url, content);
        var res = await response.Content.ReadFromJsonAsync<Response<object>>();

        response.Should().BeSuccessful();
        res.Success.Should().BeTrue();
    }

    [AutoData]
    [Theory]
    public async Task FromJson_GivenValue_Correctly(Money money)
    {
        const string url = $"{BaseUrl}/json";
        var content = JsonContent.Create(new { Amount = money.Value });
        var response = await HttpClient.PostAsync(url, content);
        var res = await response.Content.ReadFromJsonAsync<Response<object>>();

        response.Should().BeSuccessful();
        res.Success.Should().BeTrue();
    }
}
