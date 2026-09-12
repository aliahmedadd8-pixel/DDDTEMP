namespace Orders.Application.Orders.Queries.GetOrderById;

public sealed record OrderItemResponse(
    Guid Id,
    string ProductName,
    decimal UnitPrice,
    string Currency,
    int Quantity,
    decimal TotalPrice);

public sealed record OrderResponse(
    Guid Id,
    Guid CustomerId,
    string Status,
    string ShippingAddress,
    decimal TotalAmount,
    string Currency,
    IReadOnlyList<OrderItemResponse> Items,
    DateTime CreatedAtUtc);
