namespace Fabricdot.Domain.Core.Auditing
{
    public interface IHasModifierId
    {
        string LastModifierId { get; }
    }
}