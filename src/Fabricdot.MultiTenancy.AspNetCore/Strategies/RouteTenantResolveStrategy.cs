using System.Collections.Generic;
using System.Threading.Tasks;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Fabricdot.MultiTenancy.AspNetCore.Strategies
{
    public class RouteTenantResolveStrategy : HttpTenantResolveStrategy
    {
        private readonly string _routeKey;

        public RouteTenantResolveStrategy(string? routeKey = null)
        {
            _routeKey = string.IsNullOrEmpty(routeKey) ? TenantConstants.TenantToken : routeKey;
        }

        protected override Task<string?> ResolveIdentifierAsync(HttpContext httpContext)
        {
            var routeValuesFeature = httpContext.Features.Get<IRouteValuesFeature>();
            var identifier = (string?)routeValuesFeature?.RouteValues?.GetValueOrDefault(_routeKey);
            return Task.FromResult(identifier);
        }
    }
}