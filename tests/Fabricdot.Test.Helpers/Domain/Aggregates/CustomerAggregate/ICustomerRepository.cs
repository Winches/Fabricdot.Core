using Fabricdot.Domain.Services;

namespace Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;

public interface ICustomerRepository : IRepository<Customer, string>
{
}
