namespace Fabricdot.MultiTenancy;

public interface ITenant
{
    Guid Id { get; }

    string Name { get; }
}
