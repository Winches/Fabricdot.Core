using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(
            this IEnumerable<T> source,
            Action<T> action)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (action is null)
                throw new ArgumentNullException(nameof(action));

            foreach (var item in source)
                action.Invoke(item);
        }

        public static void ForEach<T>(
            this IEnumerable<T> source,
            Action<T, int> action)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (action is null)
                throw new ArgumentNullException(nameof(action));

            var index = 0;
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                action.Invoke(enumerator.Current, index++);
        }

        public static async Task ForEachAsync<T>(
            this IEnumerable<T> source,
            Func<T, int, Task> func)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (func is null)
                throw new ArgumentNullException(nameof(func));

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
            return string.Join(separator, source);
        }

        public static string JoinAsString(
            this IEnumerable<string> source,
            char separator)
        {
            return string.Join(separator, source);
        }
    }
}