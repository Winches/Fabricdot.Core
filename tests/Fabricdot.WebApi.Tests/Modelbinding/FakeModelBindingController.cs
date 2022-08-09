using System.Text.Json;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fabricdot.WebApi.Tests.Modelbinding;

[Route("api/fake/model-binding")]
[ApiController]
public class FakeModelBindingController : Controller
{
    public record TestDto(OrderStatus Status, Money Amount);

    [HttpGet("enumeration/query")]
    public void BindEnumerationFromQuery([FromQuery] OrderStatus orderStatus)
    {
        var expected = HttpContext.Request.Query[nameof(orderStatus)].ToString();

        orderStatus.Value.ToString().Should().Be(expected);
    }

    [HttpGet("enumeration/route/{orderstatus}")]
    public void BindEnumerationFromRoute([FromQuery] OrderStatus orderStatus)
    {
        var expected = HttpContext.Request.RouteValues[nameof(orderStatus)]?.ToString();

        orderStatus.Value.ToString().Should().Be(expected);
    }

    [HttpPost("enumeration/form")]
    public void BindEnumerationFromForm([FromForm] TestDto input)
    {
        var form = HttpContext.Request.Form;
        var expected = new { Status = (OrderStatus)Convert.ToInt32(form[nameof(TestDto.Status)]) };

        input.Should().BeEquivalentTo(expected);
    }

    [HttpPost("enumeration/json")]
    public void BindEnumerationFromJson([FromBody] TestDto input)
    {
        var body = HttpContext.ReadRequestBody();
        var data = body.FromJson(new { Status = 0 }, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var expected = new { Status = (OrderStatus)data.Status };

        input.Should().BeEquivalentTo(expected);
    }

    [HttpGet("singlevaleobject/query")]
    public void BindSingleVOFromQuery([FromQuery] Money money)
    {
        var expected = HttpContext.Request.Query[nameof(money)].ToString();

        money.Value.ToString().Should().Be(expected);
    }

    [HttpGet("singlevaleobject/route/{money}")]
    public void BindSingleVOFromRoute([FromQuery] Money money)
    {
        var expected = HttpContext.Request.RouteValues[nameof(money)]?.ToString();

        money.Value.ToString().Should().Be(expected);
    }

    [HttpPost("singlevaleobject/form")]
    public void BindSingleVOFromForm([FromForm] TestDto input)
    {
        var form = HttpContext.Request.Form;
        var expected = new { Amount = (Money)Convert.ToDecimal(form[nameof(TestDto.Amount)]) };

        input.Should().BeEquivalentTo(expected);
    }

    [HttpPost("singlevaleobject/json")]
    public void BindSingleVOFromJson([FromBody] TestDto input)
    {
        var body = HttpContext.ReadRequestBody();
        var data = body.FromJson(new { Amount = 0m }, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var expected = new { Amount = (Money)data.Amount };

        input.Should().BeEquivalentTo(expected);
    }
}