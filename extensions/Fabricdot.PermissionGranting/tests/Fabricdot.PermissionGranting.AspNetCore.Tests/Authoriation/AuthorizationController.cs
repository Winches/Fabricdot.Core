using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fabricdot.PermissionGranting.AspNetCore.Tests.Authoriation;

[Authorize(AuthenticationSchemes = FakeAuthenticationHandler.AuthenticationSchema)]
[Route("api/foo")]
[ApiController]
[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
public class AuthorizationController : ControllerBase
{
    [Authorize("name1")]
    [Route("shouldberejected")]
    public int GetSouldBeRejected() => 0;

    [Authorize("object1")]
    [Route("shouldbeallowed")]
    public int GetShouldBeAllowed() => 1;
}
