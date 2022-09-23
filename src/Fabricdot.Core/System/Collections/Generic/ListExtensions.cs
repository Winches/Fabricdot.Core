using Ardalis.GuardClauses;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic;

public static class ListExtensions
{
    public static void MoveItem<T>(
    this IList<T> source,
    Func<T, bool> selector,
    int targetIndex)
    {
        Guard.Against.Null(source, nameof(source));
        Guard.Against.OutOfRange(targetIndex, nameof(targetIndex), 0, source.Count - 1);

        var currentIndex = source.IndexOf(source.Single(selector));
        if (currentIndex == targetIndex)
        {
            return;
        }

        var item = source[currentIndex];
        source.RemoveAt(currentIndex);
        source.Insert(targetIndex, item);
    }
}