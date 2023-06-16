using Fabricdot.Infrastructure.Commands;

namespace Fabricdot.Infrastructure.Tests.Commands;

internal class SimpleCommand : Command
{
    public bool Invoked { get; set; }
}

internal class SimpleCommandHandler : CommandHandler<SimpleCommand>
{
    public override Task ExecuteAsync(
        SimpleCommand command,
        CancellationToken cancellationToken)
    {
        command.Invoked = true;
        return Task.CompletedTask;
    }
}
