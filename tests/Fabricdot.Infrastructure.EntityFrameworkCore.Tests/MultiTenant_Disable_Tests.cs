using Fabricdot.Domain.SharedKernel;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests;

public class MultiTenant_Disable_Tests : EntityFrameworkCoreTestsBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDataFilter _dataFilter;

    public MultiTenant_Disable_Tests()
    {
        _customerRepository = ServiceProvider.GetRequiredService<ICustomerRepository>();
        _dataFilter = ServiceProvider.GetRequiredService<IDataFilter>();
    }

    [Fact]
    public async Task EfRepository_ShouldWorkWithoutMultiTenancyModule()
    {
        using var scope = _dataFilter.Enable<IMultiTenant>();
        var employees = await _customerRepository.ListAsync();
    }
}
