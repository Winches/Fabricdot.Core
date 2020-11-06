namespace Fabricdot.Domain.Core.Auditing
{
    public interface IHasDeleterId : ISoftDelete
    {
        string DeleterId { get; }
    }
}