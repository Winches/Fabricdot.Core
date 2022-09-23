using System.Text.RegularExpressions;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;

namespace Fabricdot.MultiTenancy.AspNetCore.Strategies;

public class HostTenantResolveStrategy : HttpTenantResolveStrategy
{
    private readonly string _template;

    public HostTenantResolveStrategy(string template)
    {
        Guard.Against.NullOrEmpty(template, nameof(template));
        _template = template;// TODO:implment
    }

    protected override Task<string?> ResolveIdentifierAsync(HttpContext httpContext)
    {
        var host = httpContext.Request.Host;
        if (!host.HasValue)
            return Task.FromResult<string?>(null);

        var match = Regex.Match(
            host.Host,
            _template,
            RegexOptions.ExplicitCapture,
            TimeSpan.FromMilliseconds(100));

        var identifier = match.Success ? match.Groups["tenant"]?.Value : null;
        return Task.FromResult(identifier);
    }
}