using BuildingBlocks.Domain;

namespace Users.Domain;

public sealed record UserId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static UserId New() => new(Guid.NewGuid());
    public static UserId From(Guid value) => new(value);
}
