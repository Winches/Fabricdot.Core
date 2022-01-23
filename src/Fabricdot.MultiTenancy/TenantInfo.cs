using System;
using Fabricdot.MultiTenancy.Abstractions;

namespace Fabricdot.MultiTenancy
{
    internal class TenantInfo : ITenant
    {
        public Guid Id { get; }

        public string Name { get; }

        public TenantInfo(
            Guid tenantId,
            string tenantName)
        {
            Id = tenantId;
            Name = tenantName;
        }
    }
}