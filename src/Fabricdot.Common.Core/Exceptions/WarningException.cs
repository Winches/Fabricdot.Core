using System;

namespace Fabricdot.Common.Core.Exceptions
{
    /// <summary>
    /// </summary>
    public class WarningException : Exception
    {
        private const int DEFAULT_CODE = 1;

        /// <summary>
        ///     错误码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="message">错误消息</param>
        public WarningException(string message) : this(message, DEFAULT_CODE)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="exception">异常</param>
        public WarningException(Exception exception) : this(null, DEFAULT_CODE, exception)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误码</param>
        public WarningException(string message, int code) : this(message, code, null)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误码</param>
        /// <param name="exception">异常</param>
        public WarningException(
            string message,
            int code,
            Exception exception) : base(message ?? "", exception)
        {
            Code = code;
        }
    }
}