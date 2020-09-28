using System;
using Fabricdot.Common.Core.Exceptions;

namespace Fabricdot.WebApi.Core.Exceptions
{
    public class EndPointException : WarningException
    {
        public EndPointException(string message) : base(message)
        {
        }

        public EndPointException(string message, int code) : base(message, code)
        {
        }

        public EndPointException(
            string message,
            int code,
            Exception exception) : base(message, code, exception)
        {
        }
    }
}