using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fabricdot.Common.Core.Queryable
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