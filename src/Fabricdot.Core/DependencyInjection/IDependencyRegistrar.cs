using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.DependencyInjection;

public interface IDependencyRegistrar
{
    void Register(
        IServiceCollection services,
        Type implementationType);
}