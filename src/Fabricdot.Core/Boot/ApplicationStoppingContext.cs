using System;
using Ardalis.GuardClauses;

namespace Fabricdot.Core.Boot;

public class ApplicationStoppingContext
{
    public IServiceProvider ServiceProvider { get; }

    public ApplicationStoppingContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = Guard.Against.Null(serviceProvider, nameof(serviceProvider));
    }
}