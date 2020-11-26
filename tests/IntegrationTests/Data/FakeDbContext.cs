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

            modelBuilder.Entity<FakeEntity>()
                .HasData(new FakeEntity("26D158E4-01C4-421B-8E72-E0999DB421FC", "A"),
                    new FakeEntity("E8231719-DFBF-472C-BA9F-3898CE7852FC", "B"),
                    new FakeEntity("431DE9E8-AC1E-4ED1-9391-287D40953985", "C"),
                    new FakeEntity("538E5EE0-BB7B-495A-94B9-CC5689A1E502", "D"));
        }
    }
}