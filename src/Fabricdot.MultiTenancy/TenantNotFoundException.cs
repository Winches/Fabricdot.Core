using System;

namespace Fabricdot.MultiTenancy
{
    public class TenantNotFoundException : Exception
    {
        public TenantNotFoundException()
        {
        }

        public TenantNotFoundException(string message) : base(message)
        {
        }

        public TenantNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}