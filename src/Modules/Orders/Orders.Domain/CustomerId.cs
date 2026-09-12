using BuildingBlocks.Domain;

namespace Orders.Domain;

public sealed record CustomerId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static CustomerId New() => new(Guid.NewGuid());
    public static CustomerId From(Guid value) => new(value);
}
