using System;
using Fabricdot.Core.Aspects;

namespace Fabricdot.Infrastructure.Core.Uow
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    [InterceptorBinding]
    public class UnitOfWorkInterceptorAttribute : Attribute
    {
    }
}