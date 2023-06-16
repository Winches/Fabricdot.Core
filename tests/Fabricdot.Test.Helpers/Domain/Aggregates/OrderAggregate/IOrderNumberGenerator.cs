using Fabricdot.Domain.Services;

namespace Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

public interface IOrderNumberGenerator : IDomainService
{
    Task<string> NextAsync();
}
