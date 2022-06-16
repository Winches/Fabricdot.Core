using Ardalis.GuardClauses;
using FluentAssertions.Collections;

namespace FluentAssertions;

public static class GenericCollectionAssertionsExtensions
{
    /// <summary>
    ///     Expects the current collection to contain only a single item equals specific <paramref
    ///     name="item" /> using its <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assertions"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static AndWhichConstraint<GenericCollectionAssertions<T>, T> ContainSingle<T>(
        this GenericCollectionAssertions<T> assertions,
        T item)
    {
        Guard.Against.Null(assertions, nameof(assertions));

        return assertions.ContainSingle(v => Equals(item, v));
    }
}
