namespace Fabricdot.Infrastructure.Commands;

/// <summary>
///     command bus
/// </summary>
public interface ICommandBus
{
    Task<TResult> PublishAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default);
}
