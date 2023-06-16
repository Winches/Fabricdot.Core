namespace Fabricdot.Domain.Entities;

public interface IEntity<out TKey>
{
    TKey Id { get; }
}
