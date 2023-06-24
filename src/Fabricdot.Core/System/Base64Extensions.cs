using System.Text;
using System.Text.RegularExpressions;

namespace System;

// Source: https://stackoverflow.com/questions/11743160/how-do-i-encode-and-decode-a-base64-string
public static class Base64Extensions
{
    /// <summary>
    ///     Encodes string to base64.
    /// </summary>
    /// <param name="plainText">String to encode.</param>
    /// <param name="encoding">Encoding to use. Default to Encoding.UTF8.</param>
    /// <returns>The base64 encoded version of the string</returns>
    /// <exception cref="EncoderFallbackException"></exception>
    public static string ToBase64(
        this string plainText,
        Encoding? encoding = null)
    {
        if (string.IsNullOrWhiteSpace(plainText))
        {
            return string.Empty;
        }

        encoding ??= Encoding.UTF8;

        var stringBytes = encoding.GetBytes(plainText);
        return Convert.ToBase64String(stringBytes);
    }

    /// <summary>
    ///     Decodes string from base64 to its normal representation.
    /// </summary>
    /// <param name="encodedText">String to decode.</param>
    /// <param name="encoding">Encoding to use. Default to Encoding.UTF8.</param>
    /// <returns>The decoded version of the string</returns>
    public static string FromBase64(
        this string encodedText,
        Encoding? encoding = null)
    {
        if (string.IsNullOrEmpty(encodedText))
        {
            return string.Empty;
        }

        encodedText = encodedText.Trim();
        if (!IsBas64(encodedText))
        {
            throw new ArgumentException("Encoded text is not valid base64");
        }

        var data = Convert.FromBase64String(encodedText);
        encoding ??= Encoding.UTF8;
        return encoding.GetString(data);
    }

    private static bool IsBas64(string encodedText)
    {
        return encodedText.Length % 4 == 0
               && Regex.IsMatch(encodedText, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
    }
}
