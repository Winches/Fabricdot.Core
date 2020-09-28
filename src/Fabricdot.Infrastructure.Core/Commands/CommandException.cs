using System;
using Fabricdot.Common.Core.Exceptions;

namespace Fabricdot.Infrastructure.Core.Commands
{
    public class CommandException : WarningException
    {
        public CommandException(string message) : base(message)
        {
        }

        public CommandException(string message, int code) : base(message, code)
        {
        }

        public CommandException(
            string message,
            int code,
            Exception exception) : base(message, code, exception)
        {
        }
    }
}