using System;
using System.Security.Claims;
using Fabricdot.Common.Core.Security;

namespace IntegrationTests
{
    public class FakeCurrentUser : ICurrentUser
    {
        /// <inheritdoc />
        public bool IsAuthenticated => !string.IsNullOrEmpty(Id);

        /// <inheritdoc />
        public string Id { get; } = "C571C11D-3ED8-4AFF-B23C-2838D38FAB79";

        /// <inheritdoc />
        public string UserName { get; }

        /// <inheritdoc />
        public string[] Roles { get; }

        /// <inheritdoc />
        public Claim FindClaim(string claimType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Claim[] FindClaims(string claimType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Claim[] GetAllClaims()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool IsInRole(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}