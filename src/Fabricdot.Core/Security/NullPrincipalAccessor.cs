using System;
using System.Security.Claims;
using Fabricdot.Core.DependencyInjection;

namespace Fabricdot.Core.Security
{
    public class NullPrincipalAccessor : IPrincipalAccessor, ISingletonDependency
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