using System;

namespace Fabricdot.WebApi.Core.Exceptions
{
    public class EndPointException : Exception
    {
        public EndPointException(string message) : base(message)
        {
        }

        public EndPointException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}