namespace Fabricdot.Infrastructure.Commands;

public interface ICommandHandler
{
}

public interface ICommandHandler<TCommand> where TCommand : ICommand<object>
{
    Task ExecuteAsync(
        TCommand command,
        CancellationToken cancellationToken);
}

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> ExecuteAsync(
        TCommand command,
        CancellationToken cancellationToken);
}
