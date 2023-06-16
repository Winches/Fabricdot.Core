using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;

namespace Fabricdot.WebApi.Uow;

public static class UnitOfWorkMiddlewareExtensions
{
    public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder applicationBuilder)
    {
        Guard.Against.Null(applicationBuilder, nameof(applicationBuilder));

        applicationBuilder.UseMiddleware<UnitOfWorkMiddleware>();
        return applicationBuilder;
    }
}
