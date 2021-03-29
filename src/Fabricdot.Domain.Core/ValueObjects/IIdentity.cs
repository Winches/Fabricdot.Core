namespace Fabricdot.Domain.Core.ValueObjects
{
    public interface IIdentity<out T>
    {
        T Value { get; }
    }
}