using BuildingBlocks.Domain;

namespace Users.Domain;

public sealed record UserRegisteredDomainEvent(
    UserId UserId,
    Email Email) : IDomainEvent;
