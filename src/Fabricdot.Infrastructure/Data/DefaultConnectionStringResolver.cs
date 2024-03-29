using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.Infrastructure.Data;

public class DefaultConnectionStringResolver : IConnectionStringResolver, ITransientDependency
{
    private readonly ConnectionOptions _options;

    public DefaultConnectionStringResolver(IOptionsSnapshot<ConnectionOptions> options)
    {
        _options = options.Value;
    }

    /// <inheritdoc />
    public Task<string?> ResolveAsync(string? name = null)
    {
        return Task.FromResult(Resolve(name));
    }

    private string? Resolve(string? name)
    {
        var connectionStrings = _options.ConnectionStrings;
        if (name == null)
            return connectionStrings.Default;

        var connectionString = connectionStrings.GetValueOrDefault(name);
        return string.IsNullOrEmpty(connectionString) ? null : connectionString;
    }
}
