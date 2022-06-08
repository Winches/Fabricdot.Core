using System.Linq;
using System.Threading.Tasks;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Fabricdot.MultiTenancy.AspNetCore.Strategies;

public class QueryStringTenantResolveStrategy : HttpTenantResolveStrategy
{
    private readonly string _queryKey;

    public QueryStringTenantResolveStrategy(string? queryKey = null)
    {
        _queryKey = string.IsNullOrEmpty(queryKey) ? TenantConstants.TenantToken : queryKey;
    }

    protected override Task<string?> ResolveIdentifierAsync(HttpContext httpContext)
    {
        var query = httpContext.Request.Query;
        if (!query.ContainsKey(_queryKey))
            return Task.FromResult<string?>(null);

        var identifier = query[_queryKey];
        return Task.FromResult(identifier.FirstOrDefault());
    }
}