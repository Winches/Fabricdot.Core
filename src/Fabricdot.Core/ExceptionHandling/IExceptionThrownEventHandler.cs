namespace Fabricdot.Core.ExceptionHandling;

public interface IExceptionThrownEventHandler<in T> where T : IExceptionThrownEvent
{
    Task HandleAsync(T @event);
}