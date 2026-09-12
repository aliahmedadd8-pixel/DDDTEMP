using BuildingBlocks.Domain;

namespace Orders.Domain;

public sealed record OrderCreatedDomainEvent(
    OrderId OrderId,
    CustomerId CustomerId,
    Money TotalAmount) : IDomainEvent;
