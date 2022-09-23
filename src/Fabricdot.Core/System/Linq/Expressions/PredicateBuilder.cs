using Ardalis.GuardClauses;

namespace System.Linq.Expressions;

// From http://www.albahari.com/nutshell/predicatebuilder.aspx
public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> True<T>() => _ => true;

    public static Expression<Func<T, bool>> False<T>() => _ => false;

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        Guard.Against.Null(expr1, nameof(expr1));
        Guard.Against.Null(expr2, nameof(expr2));

        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
    }

    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        Guard.Against.Null(expr1, nameof(expr1));
        Guard.Against.Null(expr2, nameof(expr2));

        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
    }

    public static Expression<Func<T, bool>> Compose<T>(
        this IEnumerable<Expression<Func<T, bool>>> expressions,
        Func<Expression<Func<T, bool>>, Expression<Func<T, bool>>, Expression<Func<T, bool>>> @operator)
    {
        Guard.Against.NullOrEmpty(expressions, nameof(expressions));

        return expressions.Aggregate(@operator);
    }

    public static Expression<Func<T, bool>> ComposeOr<T>(this IEnumerable<Expression<Func<T, bool>>> expressions)
    {
        return expressions.Compose(Or);
    }

    public static Expression<Func<T, bool>> ComposeAnd<T>(this IEnumerable<Expression<Func<T, bool>>> expressions)
    {
        return expressions.Compose(And);
    }
}