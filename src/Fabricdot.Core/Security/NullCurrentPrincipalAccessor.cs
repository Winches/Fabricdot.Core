using System;
using System.Security.Claims;
using Fabricdot.Core.DependencyInjection;

namespace Fabricdot.Core.Security
{
    public class NullCurrentPrincipalAccessor : ICurrentPrincipalAccessor, ISingletonDependency
    {
        /// <inheritdoc />
        public ClaimsPrincipal Principal { get; } = new ClaimsPrincipal();

        /// <inheritdoc />
        public IDisposable Change(ClaimsPrincipal principal)
        {
            throw new NotSupportedException();
        }
    }
}