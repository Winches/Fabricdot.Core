namespace Fabricdot.Domain.Auditing;

public interface IHasModifierId
{
    string? LastModifierId { get; }
}