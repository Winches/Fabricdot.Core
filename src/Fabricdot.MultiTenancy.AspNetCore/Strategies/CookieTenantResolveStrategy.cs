using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Fabricdot.MultiTenancy.AspNetCore.Strategies;

public class CookieTenantResolveStrategy : HttpTenantResolveStrategy
{
    private readonly string _cookieKey;

    public CookieTenantResolveStrategy(string? cookieKey = null)
    {
        _cookieKey = string.IsNullOrEmpty(cookieKey) ? TenantConstants.TenantToken : cookieKey;
    }

    protected override Task<string?> ResolveIdentifierAsync(HttpContext httpContext)
    {
        var cookies = httpContext.Request.Cookies;
        if (!cookies.ContainsKey(_cookieKey))
            return Task.FromResult<string?>(null);

        var identifier = cookies[_cookieKey];
        return Task.FromResult(identifier);
    }
}
