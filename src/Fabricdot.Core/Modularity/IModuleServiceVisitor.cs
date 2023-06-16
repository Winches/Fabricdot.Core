using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Modularity;

public interface IModuleServiceVisitor
{
    void Visit(
        IModuleCollection modules,
        IServiceCollection services);
}
