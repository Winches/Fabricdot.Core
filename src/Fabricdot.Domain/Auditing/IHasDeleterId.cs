namespace Fabricdot.Domain.Auditing
{
    public interface IHasDeleterId : ISoftDelete
    {
        string DeleterId { get; }
    }
}