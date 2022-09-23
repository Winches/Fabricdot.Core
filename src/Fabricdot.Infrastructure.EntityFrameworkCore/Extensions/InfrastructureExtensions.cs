using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Extensions;

public static class InfrastructureExtensions
{
    public static TService GetRequiredService<TService>(this IInfrastructure<IServiceProvider> infrastructure) where TService : class
    {
        Guard.Against.Null(infrastructure, nameof(infrastructure));
        return infrastructure.GetService<TService>() ?? throw new InvalidOperationException($"No service registered for type {typeof(TService)}");
    }
}