using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tracing;

[Dependency(ServiceLifetime.Singleton)]
public class DefaultCorrelationIdProvider : ICorrelationIdProvider
{
    public CorrelationId Get() => CorrelationId.New();
}