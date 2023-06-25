using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;

namespace Fabricdot.MultiTenancy;

public class TenantResolveStrategyDecorator : ITenantResolveStrategy
{
    private readonly ILogger _logger;
    private readonly ITenantResolveStrategy _strategy;

    public int Priority { get; }

    public TenantResolveStrategyDecorator(
                ILogger logger,
        ITenantResolveStrategy strategy)
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
        _strategy = Guard.Against.Null(strategy, nameof(strategy));
    }

    public async Task<string?> ResolveIdentifierAsync(TenantResolveContext context)
    {
        if (context == null)
            return null;

        string? identifier = null;
        try
        {
            identifier = await _strategy.ResolveIdentifierAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Execute strategy failed: {Strategy}.", _strategy.GetType().PrettyPrint());
        }

        if (identifier == null)
        {
            _logger.LogDebug("Tenant identifier not found.");
        }
        else
        {
            _logger.LogDebug("Found tenant identifier: {Identifier}", identifier);
        }

        return identifier;
    }
}
