using BuildingBlocks.Domain;

namespace Orders.Domain;

public sealed record OrderCancelledDomainEvent(OrderId OrderId) : IDomainEvent;
