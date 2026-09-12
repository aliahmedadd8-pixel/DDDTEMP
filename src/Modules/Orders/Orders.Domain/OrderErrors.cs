using BuildingBlocks.Domain;

namespace Orders.Domain;

public static class OrderErrors
{
    public static Error NotFound(OrderId orderId) =>
        Error.NotFound("Order.NotFound", $"The order with ID '{orderId.Value}' was not found.");

    public static readonly Error EmptyItems =
        Error.Validation("Order.EmptyItems", "An order must contain at least one item.");

    public static readonly Error AlreadyCancelled =
        Error.Conflict("Order.AlreadyCancelled", "The order is already cancelled.");

    public static readonly Error CannotCancelShippedOrder =
        Error.Conflict("Order.CannotCancelShipped", "A shipped or delivered order cannot be cancelled.");

    public static Error CustomerNotFound(CustomerId customerId) =>
        Error.NotFound("Order.CustomerNotFound", $"Customer with ID '{customerId.Value}' does not exist in the orders system.");
}
