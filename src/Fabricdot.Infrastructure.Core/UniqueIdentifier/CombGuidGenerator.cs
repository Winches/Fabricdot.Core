using System;
using Fabricdot.Core.UniqueIdentifier;
using Microsoft.Extensions.Options;

namespace Fabricdot.Infrastructure.Core.UniqueIdentifier
{
    public class CombGuidGenerator : IGuidGenerator
    {
        private readonly CombGuidGeneratorOptions _options;

        public CombGuidGenerator(IOptions<CombGuidGeneratorOptions> options)
        {
            _options = options.Value;
        }

        public Guid Create() => GuidFactories.Comb.Create(_options.CombGuidType);
    }
}