using MediatR;
using Microsoft.Extensions.Logging;
using Orders.Domain;
using Users.Contracts;

namespace Orders.Application.Orders.Events;

public sealed class UserRegisteredIntegrationEventHandler(
    ICustomerRepository customerRepository,
    IOrdersUnitOfWork unitOfWork,
    ILogger<UserRegisteredIntegrationEventHandler> logger)
    : INotificationHandler<UserRegisteredIntegrationEvent>
{
    public async Task Handle(UserRegisteredIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Orders Module received UserRegisteredIntegrationEvent for UserId: {UserId}",
            notification.UserId);

        var customerId = CustomerId.From(notification.UserId);
        var exists = await customerRepository.ExistsAsync(customerId, cancellationToken);
        if (!exists)
        {
            var customer = Customer.Create(
                customerId,
                notification.Email,
                $"{notification.FirstName} {notification.LastName}".Trim());

            await customerRepository.AddAsync(customer, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Customer profile created in Orders module for CustomerId: {CustomerId}", customerId.Value);
        }
    }
}
