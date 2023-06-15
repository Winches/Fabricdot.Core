using Fabricdot.WebApi.Endpoint;
using Microsoft.AspNetCore.Mvc;

namespace Fabricdot.WebApi.Tests.Filters;

[Route("api/fake/result-filter")]
[ApiController]
public class FakeResultFilterController : Controller
{
    [HttpGet("failed-status")]
    public IActionResult ReturnFailedStatus(int code)
    {
        return StatusCode(code);
    }

    [HttpGet("raw-type")]
    public Response<object> ReturnResponse(
        int value,
        int code)
    {
        return new SuccessResponse(value, code);
    }
}
