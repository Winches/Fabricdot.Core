using System;
using Ardalis.GuardClauses;

namespace Fabricdot.Core.Modularity;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ExportsAttribute : Attribute
{
    public string[] Exports { get; }

    public ExportsAttribute(params string[] exports)
    {
        Exports = Guard.Against.Null(exports, nameof(exports));
    }
}