using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Users.Domain;

namespace Users.Application.Users.Events;

public sealed class UserRegisteredDomainEventHandler(
    ILogger<UserRegisteredDomainEventHandler> logger)
    : IDomainEventHandler<UserRegisteredDomainEvent>
{
    public Task Handle(UserRegisteredDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Domain Event Handled: User with ID {UserId} was registered with Email {Email}",
            domainEvent.UserId.Value,
            domainEvent.Email.Value);

        return Task.CompletedTask;
    }
}
