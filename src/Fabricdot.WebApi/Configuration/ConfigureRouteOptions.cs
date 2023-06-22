using Fabricdot.Core.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Configuration;

[ServiceContract(typeof(IConfigureOptions<RouteOptions>))]
[Dependency(ServiceLifetime.Singleton)]
public class ConfigureRouteOptions : IConfigureOptions<RouteOptions>
{
    public void Configure(RouteOptions options)
    {
        options.LowercaseUrls = true;
        options.LowercaseQueryStrings = true;
    }
}
