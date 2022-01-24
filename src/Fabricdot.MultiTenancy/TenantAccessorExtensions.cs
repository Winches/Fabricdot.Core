﻿using System;
using Ardalis.GuardClauses;
using Fabricdot.MultiTenancy.Abstractions;

namespace Fabricdot.MultiTenancy
{
    public static class TenantAccessorExtensions
    {
        public static IDisposable Change(
            this ITenantAccessor tenantAccessor,
            Guid tenantId,
            string tenantName = null)
        {
            Guard.Against.Null(tenantAccessor, nameof(tenantAccessor));

            return tenantAccessor.Change(new TenantInfo(tenantId, tenantName));
        }
    }
}