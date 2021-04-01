using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;

namespace Fabricdot.Core.Validation
{
    /// <summary>
    ///     Notification pattern
    /// </summary>
    public sealed class Notification
    {
        public readonly struct Error
        {
            private readonly string _message;

            public Error(string message)
            {
                Guard.Against.NullOrWhiteSpace(message, nameof(message));
                _message = message;
            }

            public Error Format(params object[] args) => new(string.Format(_message, args));

            /// <inheritdoc />
            public override string ToString() => _message;
        }

        /// <summary>
        ///     Error.
        /// </summary>
        private readonly IDictionary<string, IList<Error>> _errors = new Dictionary<string, IList<Error>>();

        public IReadOnlyDictionary<string, Error[]> Errors
        {
            get
            {
                return _errors
                    .ToDictionary(item => item.Key,
                        item => item.Value.ToArray());
            }
        }

        /// <summary>
        ///     Returns true when it does not contains error messages.
        /// </summary>
        public bool IsValid => _errors.Count == 0;

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        public void Add(string key, Error error)
        {
            if (!_errors.ContainsKey(key))
                _errors[key] = new List<Error>();

            _errors[key].Add(error);
        }
    }
}