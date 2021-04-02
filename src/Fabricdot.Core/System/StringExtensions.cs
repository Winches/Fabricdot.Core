using System.Linq;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        public static string[] Split(
            this string str,
            string separator,
            StringSplitOptions options = StringSplitOptions.None)
        {
            return str.Split(new[] { separator }, options);
        }


        public static string Repeat(
            this string s,
            int times,
            string separator = default)
        {
            return string.Join(separator, Enumerable.Repeat(s, times));
        }

        public static string Repeat(
            this char s,
            int times,
            string separator = default)
        {
            return string.Join(separator, Enumerable.Repeat(s, times));
        }
    }
}