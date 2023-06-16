namespace Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

public class OrderNumberGenerator : IOrderNumberGenerator
{
    public Task<string> NextAsync()
    {
        return Task.FromResult(DateTime.UtcNow.ToString());
    }
}
