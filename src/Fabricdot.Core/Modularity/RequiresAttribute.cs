using Ardalis.GuardClauses;

namespace Fabricdot.Core.Modularity;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequiresAttribute : Attribute
{
    public Type[] Requires { get; }

    public RequiresAttribute(params Type[] requires)
    {
        Guard.Against.NullOrEmpty(requires, nameof(requires));
        Requires = requires;
    }
}