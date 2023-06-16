namespace Fabricdot.Infrastructure.Aspects;

public interface IInterceptorOptions
{
    InterceptorCollection Interceptors { get; }

    List<Type> ExcludeTargets { get; }
}
