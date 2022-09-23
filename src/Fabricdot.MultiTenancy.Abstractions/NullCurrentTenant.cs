using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.MultiTenancy.Abstractions;

[Dependency(ServiceLifetime.Singleton)]
internal class NullCurrentTenant : ICurrentTenant
{
    public Guid? Id { get; }

    public string? Name { get; }

    public bool IsAvailable { get; }

    public IDisposable Change(
        Guid? tenantId,
        string? tenantName = null)
    {
        throw new NotImplementedException();
    }
}