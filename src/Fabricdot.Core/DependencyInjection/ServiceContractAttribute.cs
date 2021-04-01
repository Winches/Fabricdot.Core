using System;

namespace Fabricdot.Core.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceContractAttribute : Attribute
    {
        public Type[] ContractTypes { get; }

        public ServiceContractAttribute(params Type[] contractTypes)
        {
            ContractTypes = contractTypes;
        }
    }
}