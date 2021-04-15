namespace Fabricdot.Domain.Core.Entities
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}