using System;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories
{
    internal class AuthorRepository : EfRepository<Author, int>, IAuthorRepository
    {
        /// <inheritdoc />
        public AuthorRepository(FakeDbContext context, IServiceProvider serviceProvider) : base(context,
            serviceProvider)
        {
        }
    }
}