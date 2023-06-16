using System.Text.Json;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fabricdot.WebApi.Tests.Modelbinding;

[Route("api/fake/model-binding")]
[ApiController]
public class FakeModelBindingController : Controller
{
    [HttpGet("enumeration/query")]
    public void BindEnumerationFromQuery([FromQuery] OrderStatus orderStatus)
    {
        var values = HttpContext.Request.Query[nameof(orderStatus)];
        var expected = values.IsNullOrEmpty() ? null : (OrderStatus)Convert.ToInt32(values[0]);

        orderStatus.Should().NotBeNull().And.Be(expected);
    }

    [HttpGet("enumeration/nullable-query")]
    public void BindEnumerationFromNullableQuery([FromQuery] OrderStatus? orderStatus)
    {
        var values = HttpContext.Request.Query[nameof(orderStatus)];
        var expected = values.IsNullOrEmpty() ? null : (OrderStatus)Convert.ToInt32(values[0]);

        orderStatus.Should().Be(expected);
    }

    [HttpGet("enumeration/route/{orderstatus}")]
    public void BindEnumerationFromRoute([FromRoute] OrderStatus orderStatus)
    {
        var value = HttpContext.Request.RouteValues[nameof(orderStatus)];
        var expected = (OrderStatus)Convert.ToInt32(value?.ToString());

        orderStatus.Should().Be(expected);
    }

    [HttpPost("enumeration/form")]
    public void BindEnumerationFromForm([FromForm] TestDto input)
    {
        var values = HttpContext.Request.Form[nameof(TestDto.Status)];
        var expected = values.IsNullOrEmpty() ? null : (OrderStatus)Convert.ToInt32(values[0]);

        input.Should().NotBeNull();
        input.Status.Should().Be(expected);
    }

    [HttpPost("enumeration/json")]
    public void BindEnumerationFromJson([FromBody] TestDto input)
    {
        var body = HttpContext.ReadRequestBody();
        var data = body?.FromJson(new { Status = 0 }, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var expected = data is null ? null : new TestDto(data.Status, null);

        input.Should().NotBeNull().And.BeEquivalentTo(expected);
    }

    [HttpGet("singlevaleobject/query")]
    public void BindSingleVOFromQuery([FromQuery] Money money)
    {
        var values = HttpContext.Request.Query[nameof(money)];
        var expected = values.IsNullOrEmpty() ? null : (Money)Convert.ToDecimal(values[0]);

        money.Should().NotBeNull().And.Be(expected);
    }

    [HttpGet("singlevaleobject/nullable-query")]
    public void BindSingleVOFromNullableQuery([FromQuery] Money? money)
    {
        var values = HttpContext.Request.Query[nameof(money)];
        var expected = values.IsNullOrEmpty() ? null : (Money)Convert.ToDecimal(values[0]);

        money.Should().Be(expected);
    }

    [HttpGet("singlevaleobject/route/{money}")]
    public void BindSingleVOFromRoute([FromRoute] Money money)
    {
        var value = HttpContext.Request.RouteValues[nameof(money)];
        var expected = (Money)Convert.ToDecimal(value?.ToString());

        money.Should().NotBeNull().And.Be(expected);
    }

    [HttpPost("singlevaleobject/form")]
    public void BindSingleVOFromForm([FromForm] TestDto input)
    {
        var values = HttpContext.Request.Form[nameof(TestDto.Amount)];
        var expected = values.IsNullOrEmpty() ? null : (Money)Convert.ToDecimal(values[0]);

        input.Should().NotBeNull();
        input.Amount.Should().Be(expected);
    }

    [HttpPost("singlevaleobject/json")]
    public void BindSingleVOFromJson([FromBody] TestDto input)
    {
        var body = HttpContext.ReadRequestBody();
        var data = body?.FromJson(new { Amount = 0m }, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var expected = data is null ? null : new TestDto(null, data.Amount);

        input.Should().NotBeNull().And.BeEquivalentTo(expected);
    }
}
