namespace Fabricdot.MultiTenancy;

public class TenantResolveResult
{
    public string? Identifier { get; set; }

    public ICollection<string> Strategies { get; } = new List<string>();
}
