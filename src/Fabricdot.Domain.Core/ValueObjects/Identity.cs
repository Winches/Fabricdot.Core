namespace Fabricdot.Domain.Core.ValueObjects
{
    public abstract class Identity<T> : SingleValueObject<T>, IIdentity<T>
    {
        /// <inheritdoc />
        protected Identity(T value) : base(value)
        {
        }
    }
}