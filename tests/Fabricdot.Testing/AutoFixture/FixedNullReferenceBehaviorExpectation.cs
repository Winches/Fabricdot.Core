using System.Globalization;

namespace Fabricdot.Testing.AutoFixture;

public class FixedNullReferenceBehaviorExpectation : IBehaviorExpectation
{
    public void Verify(IGuardClauseCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.IsOptionalParameter())
            return;

        var requestedType = command.RequestedType;
        if (!requestedType.IsClass && !requestedType.IsInterface)
            return;

        try
        {
            command.Execute(null);
        }
        catch (ArgumentNullException ex)
        {
            if (string.Equals(ex.ParamName, command.RequestedParameterName, StringComparison.OrdinalIgnoreCase))
                return;

            var curtomError = string.Format(
                CultureInfo.InvariantCulture,
                "Guard Clause prevented it, however the thrown exception contains invalid parameter name. " +
                "Ensure you pass correct parameter name to the ArgumentNullException constructor." +
                "{0}Expected parameter name: {1}{0}Actual parameter name: {2}",
                Environment.NewLine,
                command.RequestedParameterName,
                ex.ParamName);
            throw command.CreateException("<null>", curtomError, ex);
        }
        catch (Exception innerException)
        {
            throw command.CreateException("null", innerException);
        }

        throw command.CreateException("null");
    }
}
