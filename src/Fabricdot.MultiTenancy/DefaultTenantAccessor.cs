using System;
using System.Threading;
using Fabricdot.Core.Delegates;
using Fabricdot.MultiTenancy.Abstractions;

namespace Fabricdot.MultiTenancy
{
    public class DefaultTenantAccessor : ITenantAccessor
    {
        private readonly AsyncLocal<ITenant> _tenant = new();

        public ITenant Tenant => _tenant.Value;

        public virtual IDisposable Change(ITenant tenant) => SetCurrent(tenant);

        protected IDisposable SetCurrent(ITenant tenant)
        {
            var parent = Tenant;
            _tenant.Value = tenant;
            return new DisposeAction(() => _tenant.Value = parent);
        }
    }
}