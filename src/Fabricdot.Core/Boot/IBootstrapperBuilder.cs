using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Boot;

public interface IBootstrapperBuilder
{
    IServiceCollection Services { get; }

    IDictionary<string, object> Properties { get; }

    IBootstrapperBuilder AddModules(Type moduleType);

    IApplication Build(IServiceProvider serviceProvider);
}
