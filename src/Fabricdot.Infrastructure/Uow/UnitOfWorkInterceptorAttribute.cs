using System;
using Fabricdot.Core.Aspects;

namespace Fabricdot.Infrastructure.Uow
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    [InterceptorBinding]
    public class UnitOfWorkInterceptorAttribute : Attribute
    {
    }
}