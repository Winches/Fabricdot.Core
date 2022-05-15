using System;
using Fabricdot.Domain.SharedKernel;
using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Tests.Entities
{
    public class User : IdentityUser, IMultiTenant
    {
        public Guid? TenantId { get; private set; }

        public User(
            Guid userId,
            string userName,
            Guid? tenantId = null) : base(userId, userName)
        {
            TenantId = tenantId;
        }

        public User(
            Guid userId,
            string userName,
            string email,
            Guid? tenantId = null) : base(userId, userName, email)
        {
            TenantId = tenantId;
        }

        private User()
        {
        }
    }
}