using System;
using Fabricdot.Common.Core.Exceptions;

namespace Fabricdot.Domain.Core.SharedKernel
{
    public class DomainException : WarningException
    {
        public DomainException(string message) : base(message)
        {
        }

        public DomainException(string message, int code) : base(message, code)
        {
        }

        public DomainException(
            string message,
            int code,
            Exception exception) : base(message, code, exception)
        {
        }
    }
}