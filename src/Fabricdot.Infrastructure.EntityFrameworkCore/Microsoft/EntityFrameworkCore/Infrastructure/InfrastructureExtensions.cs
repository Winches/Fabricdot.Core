using Ardalis.GuardClauses;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public static class InfrastructureExtensions
{
    public static TService GetRequiredService<TService>(this IInfrastructure<IServiceProvider> infrastructure) where TService : class
    {
        Guard.Against.Null(infrastructure, nameof(infrastructure));
        return infrastructure.GetService<TService>() ?? throw new InvalidOperationException($"No service registered for type {typeof(TService)}");
    }
}
