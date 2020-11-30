using System.Reflection;
using IntegrationTests.Data.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Data
{
    public class FakeDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        /// <inheritdoc />
        public FakeDbContext([NotNull] DbContextOptions<FakeDbContext> options) : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}