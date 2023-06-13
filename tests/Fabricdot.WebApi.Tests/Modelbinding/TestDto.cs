using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.WebApi.Tests.Modelbinding;

public record TestDto(OrderStatus? Status, Money? Amount);
