using System;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public class DefaultDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>, ITransientDependency
        where TDbContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultDbContextProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
        {
            var dbContext = _serviceProvider.GetRequiredService<TDbContext>();
            return Task.FromResult(dbContext);
        }
    }
}