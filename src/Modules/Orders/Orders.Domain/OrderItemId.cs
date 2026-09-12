using BuildingBlocks.Domain;

namespace Orders.Domain;

public sealed record OrderItemId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static OrderItemId New() => new(Guid.NewGuid());
    public static OrderItemId From(Guid value) => new(value);
}
