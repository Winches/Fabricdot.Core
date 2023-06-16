using AspectCore.Configuration;

namespace Fabricdot.Infrastructure.Aspects.AspectCore.Configuration;

internal static class AspectPredicateExtensions
{
    public static AspectPredicate And(this AspectPredicate left, AspectPredicate right) =>
        method => left(method) && right(method);

    public static AspectPredicate Or(this AspectPredicate left, AspectPredicate right) =>
        method => left(method) || right(method);

    public static AspectPredicate Not(this AspectPredicate predicate) => method => !predicate(method);
}
