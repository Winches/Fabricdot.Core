using Ardalis.GuardClauses;

// ReSharper disable once CheckNamespace
namespace System;

public static class IntExtensions
{
    /// <summary>
    ///     Perform action several times by given count
    /// </summary>
    /// <param name="count">Positive or zero count</param>
    /// <param name="action">action with zero-based index</param>
    public static void Times(this int count, Action<int> action)
    {
        Guard.Against.Negative(count, nameof(count));
        Guard.Against.Null(action, nameof(action));

        for (var i = 0; i < count; i++)
            action.Invoke(i);
    }
}