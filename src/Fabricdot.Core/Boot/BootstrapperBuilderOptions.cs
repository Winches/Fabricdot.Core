using Ardalis.GuardClauses;
using Fabricdot.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Boot;

public class BootstrapperBuilderOptions
{
    public IServiceCollection Services { get; }

    public ConfigurationBuilderOptions ConfigurationOptions { get; } = new();

    public BootstrapperBuilderOptions(IServiceCollection services)
    {
        Services = Guard.Against.Null(services, nameof(services));
    }

    public BootstrapperBuilderOptions() : this(new ServiceCollection())
    {
    }
}
