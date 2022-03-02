using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Fabricdot.Domain.ValueObjects;

namespace Fabricdot.Identity.Domain.Entities.UserAggregate
{
    public class IdentityUserLogin : ValueObject
    {
        public Guid UserId { get; private set; }

        public string LoginProvider { get; private set; }

        public string ProviderKey { get; private set; }

        public string ProviderDisplayName { get; private set; }

        public IdentityUserLogin(
            string loginProvider,
            string providerKey,
            string providerDisplayName)
        {
            LoginProvider = Guard.Against.NullOrEmpty(loginProvider, nameof(loginProvider));
            ProviderKey = Guard.Against.NullOrEmpty(providerKey, nameof(providerKey));
            ProviderDisplayName = providerDisplayName;
        }

        private IdentityUserLogin()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UserId;
            yield return LoginProvider;
            yield return ProviderKey;
            yield return ProviderDisplayName;
        }
    }
}