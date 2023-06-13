using System.Linq.Expressions;

namespace Fabricdot.Core.Tests.System.Linq.Expressions;

public class PredicateBuilderTests : TestBase
{
    [Fact]
    public void True_Should_BeTrue()
    {
        PredicateBuilder.True<object?>()
                        .Compile()
                        .Invoke(null)
                        .Should()
                        .BeTrue();
    }

    [Fact]
    public void False_Should_BeFalse()
    {
        PredicateBuilder.False<object?>()
                        .Compile()
                        .Invoke(null)
                        .Should()
                        .BeFalse();
    }

    [Fact]
    public void Or_GivenExpressions_Correctly()
    {
        Expression<Func<int, bool>> expr1 = v => v > 5;
        Expression<Func<int, bool>> expr2 = v => v < 2;
        var expression = expr1.Or(expr2);

        Enumerable.Range(1, 10)
                  .Where(expression.Compile())
                  .ToList()
                  .Should()
                  .OnlyContain(v => expr1.Compile().Invoke(v) || expr2.Compile().Invoke(v));
    }

    [Fact]
    public void And_GivenExpressions_Correctly()
    {
        Expression<Func<int, bool>> expr1 = v => v <= 5;
        Expression<Func<int, bool>> expr2 = v => v >= 2;
        var expression = expr1.And(expr2);

        Enumerable.Range(1, 10)
                  .Where(expression.Compile())
                  .ToList()
                  .Should()
                  .OnlyContain(v => expr1.Compile().Invoke(v) && expr2.Compile().Invoke(v));
    }

    [Fact]
    public void ComposeOr_GivenExpressions_Correctly()
    {
        Expression<Func<int, bool>> expr1 = v => v > 5;
        Expression<Func<int, bool>> expr2 = v => v < 2;
        var expressions = new Expression<Func<int, bool>>[] { expr1, expr2 };

        Enumerable.Range(1, 10)
                  .Where(expressions.ComposeOr().Compile())
                  .ToList()
                  .Should()
                  .OnlyContain(v => expr1.Compile().Invoke(v) || expr2.Compile().Invoke(v));
    }

    [Fact]
    public void ComposeAnd_GivenExpressions_Correctly()
    {
        Expression<Func<int, bool>> expr1 = v => v <= 5;
        Expression<Func<int, bool>> expr2 = v => v >= 2;
        var expressions = new Expression<Func<int, bool>>[] { expr1, expr2 };

        Enumerable.Range(1, 10)
                  .Where(expressions.ComposeAnd().Compile())
                  .ToList()
                  .Should()
                  .OnlyContain(v => expr1.Compile().Invoke(v) && expr2.Compile().Invoke(v));
    }
}
