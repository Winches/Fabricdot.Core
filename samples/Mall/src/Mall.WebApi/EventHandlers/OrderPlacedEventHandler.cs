using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Core.Events;
using Mall.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Mall.WebApi.EventHandlers
{
    internal class OrderPlacedEventHandler : IDomainEventHandler<OrderPlacedEvent>
    {
        private readonly ILogger<OrderPlacedEventHandler> _logger;

        public OrderPlacedEventHandler(ILogger<OrderPlacedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(
            OrderPlacedEvent domainEvent,
            CancellationToken cancellationToken)
        {
            var order = domainEvent.Entity;
            _logger.LogInformation($"Do something after order placed.OrderId:{order.Id}");
            return Task.CompletedTask;
        }
    }
}