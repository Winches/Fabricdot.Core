using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mall.Infrastructure.Data
{
    public sealed class DbMigrator
    {
        private readonly IServiceProvider _serviceProvider;

        public DbMigrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            await serviceProvider.GetRequiredService<AppDbContext>()
                .Database
                .MigrateAsync();
            await serviceProvider.GetRequiredService<DataBuilder>().SeedAsync();
        }
    }
}