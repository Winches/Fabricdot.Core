using System;

namespace Fabricdot.Core.Aspects
{
    /// <summary>
    ///     Indicate that an attribute is interceptor binding type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class InterceptorBindingAttribute : Attribute
    {
    }
}