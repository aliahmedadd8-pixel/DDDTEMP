using BuildingBlocks.Domain;

namespace Orders.Domain;

public sealed class Order : AggregateRoot<OrderId>, IAuditableEntity, ISoftDeletable
{
    private readonly List<OrderItem> _items = [];

    private Order(
        OrderId id,
        CustomerId customerId,
        Address shippingAddress) : base(id)
    {
        CustomerId = customerId;
        ShippingAddress = shippingAddress;
        Status = OrderStatus.Pending;
        TotalAmount = Money.Zero();
    }

    // Required for EF Core
    private Order()
    {
    }

    public CustomerId CustomerId { get; private set; } = default!;
    public OrderStatus Status { get; private set; }
    public Address ShippingAddress { get; private set; } = default!;
    public Money TotalAmount { get; private set; } = default!;
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    // IAuditableEntity
    public DateTime CreatedAtUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedAtUtc { get; set; }
    public string? LastModifiedBy { get; set; }

    // ISoftDeletable
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }

    public static Result<Order> Create(CustomerId customerId, Address shippingAddress)
    {
        var order = new Order(
            OrderId.New(),
            customerId,
            shippingAddress);

        return Result.Success(order);
    }

    public Result AddItem(string productName, Money unitPrice, int quantity)
    {
        if (Status != OrderStatus.Pending)
        {
            return Result.Failure(Error.Conflict("Order.CannotModify", "Cannot add items to an order that is no longer pending."));
        }

        var itemResult = OrderItem.Create(Id, productName, unitPrice, quantity);
        if (itemResult.IsFailure)
        {
            return Result.Failure(itemResult.Error);
        }

        _items.Add(itemResult.Value);
        RecalculateTotalAmount();

        return Result.Success();
    }

    public Result CompleteCreation()
    {
        if (_items.Count == 0)
        {
            return Result.Failure(OrderErrors.EmptyItems);
        }

        AddDomainEvent(new OrderCreatedDomainEvent(Id, CustomerId, TotalAmount));
        return Result.Success();
    }

    public Result Cancel()
    {
        if (Status == OrderStatus.Cancelled)
        {
            return Result.Failure(OrderErrors.AlreadyCancelled);
        }

        if (Status is OrderStatus.Shipped or OrderStatus.Delivered)
        {
            return Result.Failure(OrderErrors.CannotCancelShippedOrder);
        }

        Status = OrderStatus.Cancelled;
        AddDomainEvent(new OrderCancelledDomainEvent(Id));

        return Result.Success();
    }

    private void RecalculateTotalAmount()
    {
        if (_items.Count == 0)
        {
            TotalAmount = Money.Zero();
            return;
        }

        var currency = _items[0].UnitPrice.Currency;
        var total = _items.Sum(i => i.TotalPrice.Amount);
        TotalAmount = Money.Create(total, currency).Value;
    }
}
