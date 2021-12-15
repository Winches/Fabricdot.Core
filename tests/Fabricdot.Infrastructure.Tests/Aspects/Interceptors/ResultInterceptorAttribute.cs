using System;
using Fabricdot.Core.Aspects;

namespace Fabricdot.Infrastructure.Tests.Aspects.Interceptors
{
    [InterceptorBinding]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ResultInterceptorAttribute : Attribute
    {
    }
}