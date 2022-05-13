using System.Linq;
using Ardalis.GuardClauses;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        /// <summary>
        ///     Removes all items from the collection those satisfy the given <paramref
        ///     name="predicate" />.
        /// </summary>
        /// <typeparam name="T">Type of the items in the collection</typeparam>
        /// <param name="source">The collection</param>
        /// <param name="predicate">The condition to remove the items</param>
        /// <returns>List of removed items</returns>
        public static IList<T> RemoveAll<T>(
            this ICollection<T> source,
            Func<T, bool> predicate)
        {
            Guard.Against.Null(source, nameof(source));
            Guard.Against.Null(predicate, nameof(predicate));

            var items = source.Where(predicate).ToList();
            foreach (var item in items)
                source.Remove(item);

            return items;
        }

        /// <summary>
        ///     Add item when collection do not contains the target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        public static void AddIfNotContains<T>(
            this ICollection<T> source,
            T item)
        {
            Guard.Against.Null(source, nameof(source));

            if (source.Contains(item))
                return;

            source.Add(item);
        }
    }
}