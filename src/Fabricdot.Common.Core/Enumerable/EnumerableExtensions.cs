using System;
using System.Collections.Generic;

namespace Fabricdot.Common.Core.Enumerable
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            if (action is null) throw new ArgumentNullException(nameof(action));

            foreach (var item in source)
                action.Invoke(item);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            if (action is null) throw new ArgumentNullException(nameof(action));

            var index = 0;
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                action.Invoke(enumerator.Current, index++);
        }
    }
}