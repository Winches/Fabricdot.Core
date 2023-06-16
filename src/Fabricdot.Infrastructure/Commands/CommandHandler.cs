using MediatR;

namespace Fabricdot.Infrastructure.Commands;

public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>, IRequestHandler<TCommand, object> where TCommand : Command
{
    public abstract Task ExecuteAsync(
        TCommand command,
        CancellationToken cancellationToken);

    public async Task<object> Handle(
        TCommand request,
        CancellationToken cancellationToken)
    {
        await ExecuteAsync(request, cancellationToken);
        return null!;
    }
}

public abstract class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>, IRequestHandler<TCommand, TResult> where TCommand : Command<TResult>
{
    public abstract Task<TResult> ExecuteAsync(
        TCommand command,
        CancellationToken cancellationToken);

    public async Task<TResult> Handle(
        TCommand request,
        CancellationToken cancellationToken)
    {
        return await ExecuteAsync(request, cancellationToken);
    }
}
