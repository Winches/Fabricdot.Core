namespace Fabricdot.Infrastructure.Commands;

public interface ICommand
{
}

public interface ICommand<TResult> : ICommand
{
    Guid Id { get; }
}
