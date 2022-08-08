using Fabricdot.Domain.Auditing;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

public class EfRepository_DataFilter_Tests : EntityFrameworkCoreTestsBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDataFilter _dataFilter;

    public EfRepository_DataFilter_Tests()
    {
        _orderRepository = ServiceProvider.GetRequiredService<IOrderRepository>();
        _dataFilter = ServiceProvider.GetRequiredService<IDataFilter>();
    }

    [Fact]
    public async Task GetByIdAsync_GivenSoftDeletedWhenDisableSoftDelete_ReturnEntity()
    {
        using var scope = _dataFilter.Disable<ISoftDelete>();
        var actual = await _orderRepository.GetByIdAsync(FakeDataBuilder.DeletedOrderId);

        actual.Should().NotBeNull();
        actual.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_GivenSoftDeletedWhenEnableSoftDelete_ReturnNull()
    {
        using var scope = _dataFilter.Enable<ISoftDelete>();
        var actual = await _orderRepository.GetByIdAsync(FakeDataBuilder.DeletedOrderId);

        actual.Should().BeNull();
    }
}