using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Fabricdot.Domain.Core.Entities;
using Fabricdot.Domain.Core.SharedKernel;

namespace Mall.Domain.Entities.OrderAggregate
{
    public class Order : FullAuditAggregateRoot<Guid>
    {
        private List<OrderLine> _orderLines = new List<OrderLine>();

        public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();

        public Money Total { get; private set; }

        public DateTime OrderTime { get; private set; }

        public OrderStatus OrderStatus { get; private set; }

        public Address ShippingAddress { get; private set; }

        public Guid CustomerId { get; private set; }

        internal Order(
            Guid id,
            Address shippingAddress,
            Guid customerId)
        {
            Id = Guard.Against.Default(id, nameof(id));
            ShippingAddress = Guard.Against.Null(shippingAddress, nameof(shippingAddress));
            CustomerId = Guard.Against.Default(customerId, nameof(customerId));
            OrderTime = SystemClock.Now;
            OrderStatus = OrderStatus.Placed;
            Total = Money.Zero;
        }

        private Order()
        {
        }

        public void AddOrderLine(Guid productId, int quantity, Money price)
        {
            if (OrderStatus != OrderStatus.Placed)
                throw new DomainException("Order can not be modified.");

            var orderLine = _orderLines.SingleOrDefault(v => v.ProductId == productId);
            if (orderLine == null)
                _orderLines.Add(new OrderLine(productId, quantity, price));
            else
                orderLine.AddQuantity(quantity);

            Total = Calculate(_orderLines);
        }

        private static Money Calculate(ICollection<OrderLine> orderLines)
        {
            return orderLines.Sum(v => v.Price * v.Quantity);
        }
    }
}