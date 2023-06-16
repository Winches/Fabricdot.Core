namespace Fabricdot.Core.Aspects;

[DisableAspect]
public interface IInterceptor
{
    Task InvokeAsync(IInvocationContext invocationContext);
}
