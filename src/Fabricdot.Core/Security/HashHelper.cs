using System;
using System.Security.Cryptography;
using System.Text;
using Ardalis.GuardClauses;

namespace Fabricdot.Core.Security
{
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

            using var md5 = MD5.Create();
            var buffer = (encoding ?? Encoding.UTF8).GetBytes(raw);
            var hash = md5.ComputeHash(buffer);
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

            using var sha1 = SHA1.Create();
            var buffer = (encoding ?? Encoding.UTF8).GetBytes(raw);
            var hash = sha1.ComputeHash(buffer);
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
            using var sha256 = SHA256.Create();
            var buffer = (encoding ?? Encoding.UTF8).GetBytes(raw);
            var hash = sha256.ComputeHash(buffer);
            return Convert.ToHexString(hash);
        }
    }
}