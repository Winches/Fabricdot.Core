using System;
using System.Collections.Generic;

namespace Fabricdot.Infrastructure.Core.Aspects
{
    public interface IInterceptorOptions
    {
        InterceptorCollection Interceptors { get; }
        List<Type> ExcludeTargets { get; }
    }
}