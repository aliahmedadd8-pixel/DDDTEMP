using BuildingBlocks.Domain;

namespace Orders.Domain;

public sealed class OrderItem : Entity<OrderItemId>
{
    private OrderItem(
        OrderItemId id,
        OrderId orderId,
        string productName,
        Money unitPrice,
        int quantity) : base(id)
    {
        OrderId = orderId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        TotalPrice = unitPrice.Multiply(quantity).Value;
    }

    // Required for EF Core
    private OrderItem()
    {
    }

    public OrderId OrderId { get; private set; } = default!;
    public string ProductName { get; private set; } = default!;
    public Money UnitPrice { get; private set; } = default!;
    public int Quantity { get; private set; }
    public Money TotalPrice { get; private set; } = default!;

    public static Result<OrderItem> Create(
        OrderId orderId,
        string productName,
        Money unitPrice,
        int quantity)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            return Result.Failure<OrderItem>(Error.Validation("OrderItem.ProductNameEmpty", "Product name cannot be empty."));
        }

        if (quantity <= 0)
        {
            return Result.Failure<OrderItem>(Error.Validation("OrderItem.InvalidQuantity", "Quantity must be greater than zero."));
        }

        return Result.Success(new OrderItem(
            OrderItemId.New(),
            orderId,
            productName.Trim(),
            unitPrice,
            quantity));
    }
}
