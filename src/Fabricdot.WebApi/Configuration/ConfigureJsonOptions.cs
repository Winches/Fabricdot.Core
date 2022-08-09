using Fabricdot.Core.DependencyInjection;
using Fabricdot.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Configuration;

[ServiceContract(typeof(IConfigureOptions<JsonOptions>))]
[Dependency(ServiceLifetime.Singleton)]
public class ConfigureJsonOptions : IConfigureOptions<JsonOptions>
{
    public void Configure(JsonOptions options)
    {
        var converters = options.JsonSerializerOptions.Converters;
        converters.Add(new SingleValueObjectJsonConverterFactory());
        converters.Add(new EnumerationJsonConverterFactory());
    }
}