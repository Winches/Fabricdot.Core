namespace Fabricdot.Domain.ValueObjects
{
    public abstract class Identity<T> : SingleValueObject<T>, IIdentity<T>
    {
        /// <inheritdoc />
        protected Identity(T value) : base(value)
        {
        }
    }
}