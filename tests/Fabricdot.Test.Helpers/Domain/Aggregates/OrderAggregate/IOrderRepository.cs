using Fabricdot.Domain.Services;

namespace Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

public interface IOrderRepository : IRepository<Order, Guid>
{
}
