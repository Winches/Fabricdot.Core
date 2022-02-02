using System;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests
{
    public class MultiTenantTests : EntityFrameworkCoreTestsBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDataFilter _dataFilter;

        private static Guid? CurrentTenantId { get; set; }

        public MultiTenantTests()
        {
            var serviceProvider = ServiceScope.ServiceProvider;
            _employeeRepository = serviceProvider.GetService<IEmployeeRepository>();
            _dataFilter = serviceProvider.GetService<IDataFilter>();
        }

        //[Fact]
        //public async Task SaveChangesAsync_CreateEntityWithTenant_SetTenantId()
        //{
        //    var employeeId = Guid.NewGuid();
        //    CurrentTenantId = Guid.NewGuid();
        //    await _employeeRepository.AddAsync(new Employee(
        //        employeeId,
        //        "Name1"));
        //    var employee = await _employeeRepository.GetByIdAsync(employeeId);

        //    Assert.NotNull(employee);
        //    Assert.Equal(employee.TenantId, CurrentTenantId);
        //}

        //[Fact]
        //public async Task SaveChangesAsync_CreateEntityWithHost_SetNull()
        //{
        //    var employeeId = Guid.NewGuid();
        //    CurrentTenantId = null;
        //    await _employeeRepository.AddAsync(new Employee(
        //        employeeId,
        //        "Name1"));
        //    var employee = await _employeeRepository.GetByIdAsync(employeeId);

        //    Assert.NotNull(employee);
        //    Assert.Null(employee.TenantId);
        //}

        [Fact]
        public async Task ListAsync_EnableTenantFilterWithTenant_ReturnTenantData()
        {
            CurrentTenantId = FakeDataBuilder.TenantId;
            using var scope = _dataFilter.Enable<IMultiTenant>();
            var employees = await _employeeRepository.ListAsync();

            Assert.NotEmpty(employees);
            Assert.DoesNotContain(employees, v => v.TenantId != CurrentTenantId);
        }

        [Fact]
        public async Task ListAsync_EnableTenantFilterWithHost_ReturnHostData()
        {
            CurrentTenantId = null;
            using var scope = _dataFilter.Enable<IMultiTenant>();
            var employees = await _employeeRepository.ListAsync();

            Assert.NotEmpty(employees);
            Assert.DoesNotContain(employees, v => v.TenantId.HasValue);
        }

        [Fact]
        public async Task ListAsync_DisableTenantFilter_ReturnCorrectly()
        {
            CurrentTenantId = FakeDataBuilder.TenantId;
            using var scope = _dataFilter.Disable<IMultiTenant>();
            var employees = await _employeeRepository.ListAsync();

            Assert.NotEmpty(employees);
            Assert.Contains(employees, v => v.TenantId != CurrentTenantId);
        }

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var currentTenant = new Mock<ICurrentTenant>();
            currentTenant.SetupGet(v => v.Id).Returns(() => CurrentTenantId);
            serviceCollection.AddTransient(_ => currentTenant.Object);

            base.ConfigureServices(serviceCollection);
        }
    }
}