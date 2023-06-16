using System.Globalization;

namespace Fabricdot.Testing.AutoFixture;

public class WhitespaceStringBehaviorExpectation : IBehaviorExpectation
{
    public void Verify(IGuardClauseCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        if (command.RequestedType != typeof(string))
            return;

        if (command.IsOptionalParameter())
            return;

        try
        {
            command.Execute(" ");
        }
        catch (ArgumentException e2)
        {
            if (string.Equals(e2.ParamName, command.RequestedParameterName, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var customError = string.Format(
                CultureInfo.InvariantCulture,
                "Guard Clause prevented it, however the thrown exception contains invalid parameter name. " +
                "Ensure you pass correct parameter name to the ArgumentException constructor." +
                "{0}Expected parameter name: {1}{0}Actual parameter name: {2}",
                Environment.NewLine,
                command.RequestedParameterName,
                e2.ParamName);
            throw command.CreateException("<white space>", customError, e2);
        }
        catch (Exception e)
        {
            throw command.CreateException("<white space>", e);
        }
        throw command.CreateException("<white space>");
    }
}
