namespace Orders.Presentation.Contracts;

public sealed record CreateOrderItemRequest(
    string ProductName,
    decimal UnitPrice,
    string Currency,
    int Quantity);

public sealed record CreateOrderRequest(
    Guid CustomerId,
    string Street,
    string City,
    string Country,
    string ZipCode,
    List<CreateOrderItemRequest> Items);
