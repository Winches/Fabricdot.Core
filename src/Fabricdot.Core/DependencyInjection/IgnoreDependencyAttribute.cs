using System;

namespace Fabricdot.Core.DependencyInjection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
public class IgnoreDependencyAttribute : Attribute
{
}