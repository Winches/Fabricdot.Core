using Fabricdot.Domain.SharedKernel;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.MultiTenancy.Abstractions;
using Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests;

public class MultiTenantTests : EntityFrameworkCoreTestsBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDataFilter _dataFilter;

    private static Guid? CurrentTenantId { get; set; }

    public MultiTenantTests()
    {
        _customerRepository = ServiceProvider.GetRequiredService<ICustomerRepository>();
        _dataFilter = ServiceProvider.GetRequiredService<IDataFilter>();
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
        var employees = await _customerRepository.ListAsync();

        employees.Should()
                 .NotBeEmpty().And
                 .OnlyContain(v => v.TenantId == CurrentTenantId);
    }

    [Fact]
    public async Task ListAsync_EnableTenantFilterWithHost_ReturnHostData()
    {
        CurrentTenantId = null;
        using var scope = _dataFilter.Enable<IMultiTenant>();
        var employees = await _customerRepository.ListAsync();

        employees.Should()
                 .NotBeEmpty().And
                 .OnlyContain(v => !v.TenantId.HasValue);
    }

    [Fact]
    public async Task ListAsync_DisableTenantFilter_ReturnCorrectly()
    {
        CurrentTenantId = FakeDataBuilder.TenantId;
        using var scope = _dataFilter.Disable<IMultiTenant>();
        var employees = await _customerRepository.ListAsync();

        employees.Should()
                 .NotBeEmpty().And
                 .Contain(v => v.TenantId != CurrentTenantId);
    }

    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        var currentTenantMock = Mock<ICurrentTenant>();
        currentTenantMock.SetupGet(v => v.Id).Returns(() => CurrentTenantId);
        serviceCollection.AddTransient(_ => currentTenantMock.Object);

        base.ConfigureServices(serviceCollection);
    }
}