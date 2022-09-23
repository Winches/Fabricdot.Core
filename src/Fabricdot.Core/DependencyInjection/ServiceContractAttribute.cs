using Ardalis.GuardClauses;

namespace Fabricdot.Core.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ServiceContractAttribute : Attribute
{
    public Type[] ContractTypes { get; }

    public ServiceContractAttribute(params Type[] contractTypes)
    {
        Guard.Against.NullOrEmpty(contractTypes, nameof(contractTypes));
        ContractTypes = contractTypes;
    }
}