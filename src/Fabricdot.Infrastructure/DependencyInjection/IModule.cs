using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.DependencyInjection
{
    public interface IModule
    {
        void Configure(IServiceCollection services);
    }
}