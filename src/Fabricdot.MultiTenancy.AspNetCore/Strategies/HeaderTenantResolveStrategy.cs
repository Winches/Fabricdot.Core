using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Fabricdot.MultiTenancy.AspNetCore.Strategies;

public class HeaderTenantResolveStrategy : HttpTenantResolveStrategy
{
    private readonly string _headerKey;

    public HeaderTenantResolveStrategy(string? headerKey = null)
    {
        _headerKey = string.IsNullOrEmpty(headerKey) ? TenantConstants.TenantToken : headerKey;
    }

    protected override Task<string?> ResolveIdentifierAsync(HttpContext httpContext)
    {
        var headers = httpContext.Request.Headers;
        if (headers?.ContainsKey(_headerKey) != true)
            return Task.FromResult<string?>(null);

        var identifier = headers[_headerKey];
        return Task.FromResult(identifier.FirstOrDefault());
    }
}