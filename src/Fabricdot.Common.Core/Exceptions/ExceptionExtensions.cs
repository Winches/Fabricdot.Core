using System;
using System.Collections.Generic;
using System.Text;

namespace Fabricdot.Common.Core.Exceptions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     获取异常列表
        /// </summary>
        /// <param name="exception">异常</param>
        public static IEnumerable<Exception> GetExceptions(this Exception exception)
        {
            var result = new List<Exception>();
            AddException(result, exception);
            return result;
        }

        /// <summary>
        ///     获取错误消息
        /// </summary>
        public static string GetMessage(this Exception exception)
        {
            var result = new StringBuilder();
            var list = exception.GetExceptions();
            foreach (var item in list)
                AppendMessage(result, item);

            return result.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }

        /// <summary>
        ///     添加内部异常
        /// </summary>
        private static void AddException(List<Exception> result, Exception exception)
        {
            while (true)
            {
                if (exception == null) return;

                result.Add(exception);
                exception = exception.InnerException;
            }
        }

        /// <summary>
        ///     添加异常消息
        /// </summary>
        private static void AppendMessage(StringBuilder result, Exception exception)
        {
            if (exception == null)
                return;
            result.AppendLine(exception.Message);
        }
    }
}