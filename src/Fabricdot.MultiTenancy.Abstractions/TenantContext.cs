using Ardalis.GuardClauses;

namespace Fabricdot.MultiTenancy.Abstractions;

public class TenantContext : ITenant
{
    public Guid Id { get; }

    public string Name { get; }

    public IDictionary<string, string> ConnectionStrings { get; } = new Dictionary<string, string>();

    public TenantContext(
        Guid tenantId,
        string tenantName)
    {
        Id = tenantId;
        Name = Guard.Against.NullOrEmpty(tenantName, nameof(tenantName));
    }
}
