using System;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

internal class EmployeeRepository : EfRepository<FakeDbContext, Employee, Guid>, IEmployeeRepository
{
    /// <inheritdoc />
    public EmployeeRepository(IDbContextProvider<FakeDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}