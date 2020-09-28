using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fabricdot.Common.Core.Exceptions;

namespace Fabricdot.WebApi.Core.Exceptions
{
    public class ValidationException : WarningException
    {
        private const int DEFAULT_CODE = 10;

        public IEnumerable<ValidationResult> Errors { get; }

        public ValidationException(string message) : base(message, DEFAULT_CODE)
        {
            Errors = new List<ValidationResult>();
        }

        public ValidationException(string message, IEnumerable<ValidationResult> errors) : base(message, DEFAULT_CODE)
        {
            Errors = errors;
        }

        public ValidationException(string message, Exception exception) : base(message, DEFAULT_CODE, exception)
        {
            Errors = new List<ValidationResult>();
        }
    }
}