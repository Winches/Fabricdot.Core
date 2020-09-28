using System;
using System.Security.Cryptography;
using System.Text;
using Fabricdot.Common.Core.Enumerable;

namespace Fabricdot.Common.Core.Security
{
    public static class HashExtensions
    {
        /// <summary>
        ///     MD5 Hash
        /// </summary>
        /// <param name="raw"></param>
        /// <returns>upper case</returns>
        public static string ToMd5(this string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                throw new ArgumentException($"'{nameof(raw)}' cannot be null or whitespace", nameof(raw));

            using var md5 = new MD5CryptoServiceProvider();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(raw));
            var sb = new StringBuilder(bs.Length * 2);
            bs.ForEach(v => sb.Append(v.ToString("x2")));
            return sb.ToString().ToUpper();
        }

        /// <summary>
        ///     SHA1 Hash
        /// </summary>
        /// <param name="raw"></param>
        /// <returns>upper case</returns>
        public static string ToSha1(this string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                throw new ArgumentException($"'{nameof(raw)}' cannot be null or whitespace", nameof(raw));

            using var sha1 = new SHA1CryptoServiceProvider();
            var bs = sha1.ComputeHash(Encoding.UTF8.GetBytes(raw));
            var sb = new StringBuilder(bs.Length * 2);
            bs.ForEach(v => sb.Append(v.ToString("x2")));
            return sb.ToString().ToUpper();
        }
    }
}