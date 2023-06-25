using Ardalis.GuardClauses;

namespace System;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string? str) => string.IsNullOrEmpty(str);

    public static bool IsNullOrWhiteSpace(this string? str) => string.IsNullOrWhiteSpace(str);

    public static string Repeat(
        this string s,
        int times,
        string? separator = default)
    {
        Guard.Against.Negative(times, nameof(times));

        return string.Join(separator, Enumerable.Repeat(s, times));
    }

    public static string Repeat(
        this char s,
        int times,
        string? separator = default)
    {
        Guard.Against.Negative(times, nameof(times));

        return string.Join(separator, Enumerable.Repeat(s, times));
    }

    /// <summary>
    ///     Truncates string so that it is no longer than the specified number of characters.
    /// </summary>
    /// <param name="input">String to truncate.</param>
    /// <param name="length">Maximum string length.</param>
    /// <returns>Original string or a truncated one if the original was too long.</returns>
    public static string Truncate(
        this string input,
        int length)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be >= 0");
        }

        var maxLength = Math.Min(input.Length, length);
        return input[..maxLength];
    }
}
