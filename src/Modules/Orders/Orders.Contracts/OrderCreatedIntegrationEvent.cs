using BuildingBlocks.Application;

namespace Orders.Contracts;

public sealed record OrderCreatedIntegrationEvent(
    Guid OrderId,
    Guid CustomerId,
    decimal TotalAmount,
    string Currency) : IIntegrationEvent;
