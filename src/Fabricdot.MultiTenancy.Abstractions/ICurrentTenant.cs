namespace Fabricdot.MultiTenancy;

public interface ICurrentTenant
{
    Guid? Id { get; }

    string? Name { get; }

    bool IsAvailable { get; }

    IDisposable Change(
        Guid? tenantId,
        string? tenantName = null);
}
