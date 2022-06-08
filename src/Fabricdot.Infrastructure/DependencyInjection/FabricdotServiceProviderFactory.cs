using System;
using AspectCore.Extensions.DependencyInjection;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.DependencyInjection;

public class FabricdotServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
{
    private readonly IServiceProviderFactory<IServiceCollection> _subject = new DynamicProxyServiceProviderFactory();

    public IServiceCollection CreateBuilder(IServiceCollection services)
    {
        return _subject.CreateBuilder(services);
    }

    public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
    {
        containerBuilder.AddInterceptors(opts => opts.ExcludeTargets.Add(typeof(IAmbientUnitOfWork)));
        return _subject.CreateServiceProvider(containerBuilder);
    }
}