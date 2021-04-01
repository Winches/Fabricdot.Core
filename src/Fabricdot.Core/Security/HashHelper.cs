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
        /// <returns>upper case</returns>
        public static string ToMd5(string raw)
        {
            Guard.Against.NullOrWhiteSpace(raw, nameof(raw));

            using var md5 = new MD5CryptoServiceProvider();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(raw));
            var sb = new StringBuilder(bs.Length * 2);
            Array.ForEach(bs, v => sb.Append(v.ToString("x2")));
            return sb.ToString().ToUpper();
        }

        /// <summary>
        ///     SHA1 Hash
        /// </summary>
        /// <param name="raw"></param>
        /// <returns>upper case</returns>
        public static string ToSha1(string raw)
        {
            Guard.Against.NullOrWhiteSpace(raw, nameof(raw));

            using var sha1 = new SHA1CryptoServiceProvider();
            var bs = sha1.ComputeHash(Encoding.UTF8.GetBytes(raw));
            var sb = new StringBuilder(bs.Length * 2);
            Array.ForEach(bs, v => sb.Append(v.ToString("x2")));
            return sb.ToString().ToUpper();
        }
    }
}