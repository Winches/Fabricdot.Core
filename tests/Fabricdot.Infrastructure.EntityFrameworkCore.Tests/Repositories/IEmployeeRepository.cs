using System;
using Fabricdot.Domain.Services;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
    }
}