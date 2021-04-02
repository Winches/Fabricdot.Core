using System.Linq.Expressions;

// ReSharper disable once CheckNamespace
namespace System.Linq
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(
            IQueryable<T> query,
            Expression<Func<bool>> condition,
            Expression<Func<T, bool>> predicate)
        {
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            return condition.Compile().Invoke() ? query.Where(predicate) : query;
        }
    }
}