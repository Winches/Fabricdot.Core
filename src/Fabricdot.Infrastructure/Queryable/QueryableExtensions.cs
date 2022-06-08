using System.Linq;
using Ardalis.GuardClauses;

namespace Fabricdot.Infrastructure.Queryable;

public static class QueryableExtensions
{
    public static IQueryable<T> Paging<T>(
        this IQueryable<T> query,
        int index,
        int size)
    {
        Guard.Against.Null(query, nameof(query));
        Guard.Against.NegativeOrZero(index, nameof(index));
        Guard.Against.NegativeOrZero(size, nameof(size));

        return query.Skip((index - 1) * size).Take(size);
    }
}