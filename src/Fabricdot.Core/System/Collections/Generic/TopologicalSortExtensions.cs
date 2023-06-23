using Ardalis.GuardClauses;

namespace System.Collections.Generic;

public static class TopologicalSortExtensions
{
    /// <summary>
    ///     Sort a list by a topological sorting, which consider their dependencies.
    /// </summary>
    /// <typeparam name="T">The type of the members of values.</typeparam>
    /// <param name="source">A list of objects to sort</param>
    /// <param name="dependenciesSelector">Function to resolve the dependencies</param>
    /// <param name="comparer">Equality comparer for dependencies</param>
    /// <returns>
    ///     Returns a new list ordered by dependencies. If A depends on B, then B will come before
    ///     than A in the resulting list.
    /// </returns>
    public static ICollection<T> TopologicalSort<T>(
        this IEnumerable<T> source,
        Func<T, IEnumerable<T>> dependenciesSelector,
        IEqualityComparer<T>? comparer = null) where T : notnull
    {
        /* See: http://www.codeproject.com/Articles/869059/Topological-sorting-in-Csharp
         *      http://en.wikipedia.org/wiki/Topological_sorting
         */

        Guard.Against.Null(source, nameof(source));
        Guard.Against.Null(dependenciesSelector, nameof(dependenciesSelector));

        var sorted = new List<T>();
        var visited = new Dictionary<T, bool>(comparer);

        foreach (var item in source)
        {
            SortByDependenciesVisit(
                item,
                dependenciesSelector,
                sorted,
                visited);
        }

        return sorted;
    }

    /// <summary>
    ///     Sort by visited dependencies
    /// </summary>
    /// <typeparam name="T">The type of the members of values.</typeparam>
    /// <param name="item">Item to resolve</param>
    /// <param name="dependenciesSelector">Function to resolve the dependencies</param>
    /// <param name="sorted">List with the sortet items</param>
    /// <param name="visited">Dictionary with the visited items</param>
    private static void SortByDependenciesVisit<T>(
        T item,
        Func<T, IEnumerable<T>> dependenciesSelector,
        ICollection<T> sorted,
        Dictionary<T, bool> visited) where T : notnull
    {
        var alreadyVisited = visited.TryGetValue(item, out var inProcess);

        if (alreadyVisited)
        {
            if (inProcess)
            {
                throw new ArgumentException("Cyclic dependency found! Item: " + item);
            }
        }
        else
        {
            visited[item] = true;

            var dependencies = dependenciesSelector(item);
            if (dependencies != null)
            {
                foreach (var dependency in dependencies)
                {
                    SortByDependenciesVisit(dependency, dependenciesSelector, sorted, visited);
                }
            }

            visited[item] = false;
            sorted.Add(item);
        }
    }
}
