using System.Reflection;

namespace Fabricdot.Core.Aspects;

[DisableAspect]
public interface IInvocationContext
{
    object TargetObject { get; }

    MethodInfo Method { get; }

    object[] Parameters { get; }

    object ReturnValue { get; set; }

    IDictionary<object, object> Properties { get; }

    Task ProceedAsync();
}
