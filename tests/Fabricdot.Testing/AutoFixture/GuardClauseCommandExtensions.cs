using Ardalis.GuardClauses;

namespace Fabricdot.Testing.AutoFixture;

public static class GuardClauseCommandExtensions
{
    public static bool IsOptionalParameter(this IGuardClauseCommand command)
    {
        Guard.Against.Null(command, nameof(command));

        return command is ReflectionExceptionUnwrappingCommand unwrappingCommand
               && unwrappingCommand.Command is MethodInvokeCommand invokeCommand
               && invokeCommand.ParameterInfo.IsOptional;
    }
}