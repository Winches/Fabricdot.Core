using System.Security.Cryptography;
using System.Text;
using Ardalis.GuardClauses;

namespace Fabricdot.Core.Security;

public static class HashHelper
{
    /// <summary>
    ///     MD5 Hash
    /// </summary>
    /// <param name="raw"></param>
    /// <param name="encoding">UTF8 is default encoding</param>
    /// <returns>upper case</returns>
    public static string ToMd5(
        string raw,
        Encoding? encoding = null)
    {
        Guard.Against.Null(raw, nameof(raw));
        var buffer = (encoding ?? Encoding.UTF8).GetBytes(raw);
        var hash = MD5.HashData(buffer);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    ///     SHA1 Hash
    /// </summary>
    /// <param name="raw"></param>
    /// <param name="encoding">UTF8 is default encoding</param>
    /// <returns>upper case</returns>
    public static string ToSha1(
        string raw,
        Encoding? encoding = null)
    {
        Guard.Against.Null(raw, nameof(raw));
        var buffer = (encoding ?? Encoding.UTF8).GetBytes(raw);
        var hash = SHA1.HashData(buffer);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    ///     SHA256 Hash
    /// </summary>
    /// <param name="raw"></param>
    /// <param name="encoding">UTF8 is default encoding</param>
    /// <returns></returns>
    public static string ToSha256(
        string raw,
        Encoding? encoding = null)
    {
        var buffer = (encoding ?? Encoding.UTF8).GetBytes(raw);
        var hash = SHA256.HashData(buffer);
        return Convert.ToHexString(hash);
    }
}
