using System.Collections.Generic;
using Fabricdot.MultiTenancy.Abstractions;
using Fabricdot.MultiTenancy.Strategies;

namespace Fabricdot.MultiTenancy;

public class MultiTenancyOptions
{
    public ICollection<ITenantResolveStrategy> ResolveStrategies { get; } = new List<ITenantResolveStrategy>();

    public MultiTenancyOptions()
    {
        ResolveStrategies.Add(new PrincipalTenantResolveStrategy());
    }
}