using Fabricdot.Core.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Configuration;

[ServiceContract(typeof(IConfigureOptions<ApiBehaviorOptions>))]
[Dependency(ServiceLifetime.Singleton)]
public class ConfigureApiBehaviorOptions : IConfigureOptions<ApiBehaviorOptions>
{
    public void Configure(ApiBehaviorOptions options)
    {
        options.SuppressModelStateInvalidFilter = true;
    }
}