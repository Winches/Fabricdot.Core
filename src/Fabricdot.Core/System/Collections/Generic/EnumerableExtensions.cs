using Ardalis.GuardClauses;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic;

public static class EnumerableExtensions
{
    /// <summary>
    ///     Checks enumerable object is null or has no item.
    /// </summary>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
    {
        return source switch
        {
            ICollection<T> collection => source == null || collection.Count == 0,
            _ => source?.Any() != true
        };
    }

    public static void ForEach<T>(
            this IEnumerable<T> source,
            Action<T> action)
    {
        Guard.Against.Null(source, nameof(source));
        Guard.Against.Null(action, nameof(action));

        foreach (var item in source)
            action.Invoke(item);
    }

    public static void ForEach<T>(
        this IEnumerable<T> source,
        Action<T, int> action)
    {
        Guard.Against.Null(source, nameof(source));
        Guard.Against.Null(action, nameof(action));

        var index = 0;
        using var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
            action.Invoke(enumerator.Current, index++);
    }

    public static async Task ForEachAsync<T>(
        this IEnumerable<T> source,
        Func<T, int, Task> func)
    {
        Guard.Against.Null(source, nameof(source));
        Guard.Against.Null(func, nameof(func));

        var index = 0;
        using var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var task = func.Invoke(enumerator.Current, index++);
            await task.ConfigureAwait(false);
        }
    }

    public static string JoinAsString(
        this IEnumerable<string> source,
        string separator)
    {
        Guard.Against.Null(source, nameof(source));

        return string.Join(separator, source);
    }

    public static string JoinAsString(
        this IEnumerable<string> source,
        char separator)
    {
        Guard.Against.Null(source, nameof(source));

        return string.Join(separator, source);
    }
}
