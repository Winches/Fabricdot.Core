namespace Fabricdot.Domain.Core.Entities
{
    public interface IEntity<out TKey> : IHasDomainEvents
    {
        TKey Id { get; }
    }
}