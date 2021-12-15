using System.Collections.Generic;

namespace Fabricdot.Domain.ValueObjects
{
    public class SingleValueObject<T> : ValueObject
    {
        public T Value { get; protected set; }

        protected SingleValueObject(T value)
        {
            Value = value;
        }

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}