using Ardalis.GuardClauses;

// ReSharper disable once CheckNamespace
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
}