using System.Threading.Tasks;
using Fabricdot.Domain.SharedKernel;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests
{
    public class MultiTenant_DisableTests : EntityFrameworkCoreTestsBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDataFilter _dataFilter;

        public MultiTenant_DisableTests()
        {
            var serviceProvider = ServiceScope.ServiceProvider;
            _employeeRepository = serviceProvider.GetService<IEmployeeRepository>();
            _dataFilter = serviceProvider.GetService<IDataFilter>();
        }

        [Fact]
        public async Task EfRepository_ShouldWorkWithoutMultiTenancyModule()
        {
            using var scope = _dataFilter.Enable<IMultiTenant>();
            var employees = await _employeeRepository.ListAsync();
        }
    }
}