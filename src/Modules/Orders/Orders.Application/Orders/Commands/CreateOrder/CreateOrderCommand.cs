using BuildingBlocks.Application;

namespace Orders.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderItemDto(
    string ProductName,
    decimal UnitPrice,
    string Currency,
    int Quantity);

public sealed record CreateOrderCommand(
    Guid CustomerId,
    string Street,
    string City,
    string Country,
    string ZipCode,
    List<CreateOrderItemDto> Items) : ICommand<Guid>;
