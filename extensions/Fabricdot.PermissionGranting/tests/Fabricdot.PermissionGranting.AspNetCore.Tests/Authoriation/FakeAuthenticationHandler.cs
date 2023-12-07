using System.Security.Claims;
using System.Text.Encodings.Web;
using Fabricdot.PermissionGranting.Tests.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fabricdot.PermissionGranting.AspNetCore.Tests.Authoriation;

public class FakeAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string AuthenticationSchema = "fake";

    public FakeAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Fake user"),
            new Claim(ClaimTypes.NameIdentifier, FakeDataBuilder.Subject.Value)
        };
        var identity = new ClaimsIdentity(claims, AuthenticationSchema);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationSchema);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
