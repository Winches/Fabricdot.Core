namespace Fabricdot.Infrastructure.Data;

public interface IConnectionStringResolver
{
    Task<string?> ResolveAsync(string? name = null);
}