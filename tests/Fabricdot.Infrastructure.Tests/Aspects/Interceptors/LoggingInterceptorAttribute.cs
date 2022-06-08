using System;
using Fabricdot.Core.Aspects;

namespace Fabricdot.Infrastructure.Tests.Aspects.Interceptors;

[InterceptorBinding]
[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
internal class LoggingInterceptorAttribute : Attribute
{
}