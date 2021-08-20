using System.Collections.Generic;
using System.Threading.Tasks;
using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.Infrastructure.Core.Data
{
    public class DefaultConnectionStringResolver : IConnectionStringResolver, ITransientDependency
    {
        private readonly DbConnectionOptions _options;

        public DefaultConnectionStringResolver(IOptionsSnapshot<DbConnectionOptions> options)
        {
            _options = options.Value;
        }

        /// <inheritdoc />
        public Task<string> ResolveAsync(string name = null)
        {
            return Task.FromResult(Resolve(name));
        }

        private string Resolve(string name)
        {
            var connectionStrings = _options.ConnectionStrings;
            if (name == null)
                return connectionStrings.Default;

            var connectionString = connectionStrings.GetValueOrDefault(name);
            return string.IsNullOrEmpty(connectionString) ? null : connectionString;
        }
    }
}