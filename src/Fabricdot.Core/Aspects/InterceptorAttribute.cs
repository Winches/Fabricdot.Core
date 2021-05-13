using System;

namespace Fabricdot.Core.Aspects
{
    /// <summary>
    ///     Specific interceptor meta data
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class InterceptorAttribute : Attribute
    {
        public int Order { get; set; }
        public Type Target { get; set; }
    }
}