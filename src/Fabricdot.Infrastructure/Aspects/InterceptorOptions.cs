using System;
using System.Collections.Generic;

namespace Fabricdot.Infrastructure.Aspects;

public class InterceptorOptions : IInterceptorOptions
{
    /// <inheritdoc />
    public InterceptorCollection Interceptors { get; }

    /// <inheritdoc />
    public List<Type> ExcludeTargets { get; }

    public InterceptorOptions()
    {
        Interceptors = new InterceptorCollection();
        ExcludeTargets = new List<Type>();
    }
}