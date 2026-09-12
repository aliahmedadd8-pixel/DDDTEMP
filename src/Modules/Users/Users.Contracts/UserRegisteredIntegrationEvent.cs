using BuildingBlocks.Application;

namespace Users.Contracts;

public sealed record UserRegisteredIntegrationEvent(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName) : IIntegrationEvent;
