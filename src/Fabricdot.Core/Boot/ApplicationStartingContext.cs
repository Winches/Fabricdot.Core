using System;
using Ardalis.GuardClauses;

namespace Fabricdot.Core.Boot
{
    public class ApplicationStartingContext
    {
        public IServiceProvider ServiceProvider { get; }

        public ApplicationStartingContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = Guard.Against.Null(serviceProvider, nameof(serviceProvider));
        }
    }
}