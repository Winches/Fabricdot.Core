using System;
using Fabricdot.Core.Aspects;

namespace Fabricdot.Infrastructure.Core.Tests.Aspects.Interceptors
{
    [InterceptorBinding]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ResultInterceptorAttribute : Attribute
    {
    }
}