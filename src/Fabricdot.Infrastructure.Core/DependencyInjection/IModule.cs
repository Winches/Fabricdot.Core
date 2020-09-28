using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Core.DependencyInjection
{
    public interface IModule
    {
        void Configure(IServiceCollection services);
    }
}