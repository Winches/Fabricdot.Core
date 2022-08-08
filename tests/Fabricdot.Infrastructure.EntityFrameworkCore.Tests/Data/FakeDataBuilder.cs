using AutoFixture;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;

public class FakeDataBuilder : ITransientDependency
{
    private readonly FakeDbContext _dbContext;
    private readonly FakeSecondDbContext _secondDbContext;
    private readonly IFixture _fixture;

    public static int DeletedAuthorId => 2;
    public static string BookWithTagsId => "f00015fe-e5a7-419a-a235-a897a5f7df8c";
    public static string DeletedBookTag => "DeletedTag";
    public static Guid TenantId => new("86b2b1b1-ef3d-46e2-a4c6-5a1df6f694d4");

    public static Guid OrderId => new("07385b8c-2cbd-4fca-878e-2770ab0abe30");

    public static Guid DeletedOrderId => new("71b583f9-780d-4095-b1ca-03cbbea6c988");

    public FakeDataBuilder(
        FakeDbContext dbContext,
        FakeSecondDbContext secondDbContext,
        IFixture fixture)
    {
        _dbContext = dbContext;
        _secondDbContext = secondDbContext;
        _fixture = fixture;
    }

    public async Task BuildAsync()
    {
        await AddOrdersAsync();
        await AddCustomersAsync();
    }

    private async Task AddOrdersAsync()
    {
        var orders = _fixture.CreateMany<Order>(10).ToList();
        orders.Add(factory(OrderId));
        orders.Add(factory(DeletedOrderId));
        foreach (var order in orders)
        {
            order.AddOrderLine(_fixture);
            order.AddOrderLine(_fixture);
        }
        await _dbContext.AddRangeAsync(orders);
        await _dbContext.SaveChangesAsync();

        _dbContext.Remove(orders.Find(v => v.Id == DeletedOrderId));
        await _dbContext.SaveChangesAsync();

        Order factory(Guid id) => new(id, _fixture.Create<Address>(), _fixture.Create<string>(), _fixture.Create<OrderDetails>());
    }

    private async Task AddCustomersAsync()
    {
        var customers = _fixture.CreateMany<Customer>(5).ToList();
        customers.Add(factory());
        customers.Add(factory());

        await _secondDbContext.AddRangeAsync(customers);
        await _secondDbContext.SaveChangesAsync();

        Customer factory() => new(_fixture.Create<Guid>(), _fixture.Create<string>(), TenantId);
    }
}