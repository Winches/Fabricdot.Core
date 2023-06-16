namespace Fabricdot.Domain.ValueObjects;

public interface IIdentity<out T>
{
    T Value { get; }
}
