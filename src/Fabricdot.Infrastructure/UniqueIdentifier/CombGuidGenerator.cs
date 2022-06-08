using System;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Core.UniqueIdentifier;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.Infrastructure.UniqueIdentifier;

[Dependency(ServiceLifetime.Singleton)]
public class CombGuidGenerator : IGuidGenerator
{
    private readonly CombGuidGeneratorOptions _options;

    public CombGuidGenerator(IOptions<CombGuidGeneratorOptions> options)
    {
        _options = options.Value;
    }

    public Guid Create() => GuidFactories.Comb.Create(_options.CombGuidType);
}